﻿using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using SteamTools.Core.Models;
using SteamTools.Core.Models.Steam;
using SteamTools.Core.Services;
using SteamTools.Core.Utilities;
using SteamTools.IDScanner.Services;
using SteamTools.UI.Models;
using ByteConverter = SteamTools.Core.Utilities.ByteConverter;

namespace SteamTools.UI.ViewModels;

public class IDScannerViewModel : ObservableObject
{
    private readonly CollectionViewSource _filteredCollectionViewSource;
    private readonly INotificationService _notificationService;
    private readonly IServiceProvider _serviceProvider;
    private readonly ISteamClient _steamClient;
    private ObservableCollection<SearchExtension> _availableExtensions;
    private CancellationTokenSource _cancellationTokenSource;
    private string _extensionQuery;
    private bool _isScanning;
    private bool _limitFileSize = true;
    private ObservableCollection<string> _matchingFiles;
    private int _maxFileSizeInMb = 1;
    private string _scanQuery;
    private bool _useSelectedExtensions;

    public IDScannerViewModel(ISteamClient steamClient, INotificationService notificationService,
        IServiceProvider serviceProvider)
    {
        _steamClient = steamClient;
        _serviceProvider = serviceProvider;
        _filteredCollectionViewSource = new CollectionViewSource();
        _availableExtensions = new ObservableCollection<SearchExtension>();
        _notificationService = notificationService;
        _cancellationTokenSource = new CancellationTokenSource();

        LoadSearchExtensionsAsync();

        DecreaseMaximumFileSizeCommand = new RelayCommand(() => MaxFileSizeInMb--);
        IncreaseMaximumFileSizeCommand = new RelayCommand(() => MaxFileSizeInMb++);
        SelectAllSearchExtensionsCommand = new RelayCommand(() => ChangeSelected(true));
        ResetAllSearchExtensionsCommand = new RelayCommand(() => ChangeSelected(false));
        RunScanCommand = new AsyncRelayCommand(RunScanAsync);
        CancelScanCommand = new RelayCommand(CancelScan);
        OpenInExplorerCommand = new RelayCommand<string>(OpenInExplorer);
        MatchingFiles = new ObservableCollection<string>();
    }

    /// <summary>
    ///     Provides access to the filtered view of the available extensions.
    /// </summary>
    public ICollectionView FilteredCollectionViewSource => _filteredCollectionViewSource.View;

    /// <summary>
    ///     Gets or sets the collection of matching files found during the scan.
    /// </summary>
    public ObservableCollection<string> MatchingFiles
    {
        get => _matchingFiles;
        private set
        {
            _matchingFiles = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    ///     Gets or sets the search query entered by the user.
    /// </summary>
    public string ScanQuery
    {
        get => _scanQuery;
        set
        {
            if (_scanQuery == value) return;

            _scanQuery = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    ///     Gets or sets the filter query used to filter the available extensions.
    /// </summary>
    public string ExtensionQuery
    {
        get => _extensionQuery;
        set
        {
            if (_extensionQuery == value) return;

            _extensionQuery = value;
            OnPropertyChanged();
            _filteredCollectionViewSource.View.Refresh();
        }
    }

    /// <summary>
    ///     Gets or sets a value indicating whether the search should limit the file size.
    /// </summary>
    public bool LimitFileSize
    {
        get => _limitFileSize;
        set
        {
            if (_limitFileSize == value) return;

            _limitFileSize = value;
            if (value is false) _notificationService.ShowNotification("Life is short, don't disable limits!");
            OnPropertyChanged();
        }
    }

    /// <summary>
    ///     Gets or sets the maximum file size in megabytes allowed during the search.
    /// </summary>
    public int MaxFileSizeInMb
    {
        get => _maxFileSizeInMb;
        set
        {
            if (_maxFileSizeInMb == value) return;

            _maxFileSizeInMb = Math.Min(Math.Max(0, value), 1024);
            if (value > 5) _notificationService.ShowNotification("Size matters, limit it wisely!");
            OnPropertyChanged();
        }
    }

    /// <summary>
    ///     Gets or sets a value indicating whether the search should use only selected file extensions.
    /// </summary>
    public bool UseSelectedExtensions
    {
        get => _useSelectedExtensions;
        set
        {
            if (_useSelectedExtensions == value) return;

            _useSelectedExtensions = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    ///     Gets the command to run the scan asynchronously.
    /// </summary>
    public AsyncRelayCommand RunScanCommand { get; }

    /// <summary>
    ///     Gets the command to cancel the ongoing scan.
    /// </summary>
    public RelayCommand CancelScanCommand { get; }

    /// <summary>
    ///     Gets the command to select all file extensions for the scan.
    /// </summary>
    public RelayCommand SelectAllSearchExtensionsCommand { get; }

    /// <summary>
    ///     Gets the command to reset all file extensions for the scan.
    /// </summary>
    public RelayCommand ResetAllSearchExtensionsCommand { get; }

    /// <summary>
    ///     Gets the command to decrease the maximum file size allowed during the search.
    /// </summary>
    public RelayCommand DecreaseMaximumFileSizeCommand { get; }

    /// <summary>
    ///     Gets the command to increase the maximum file size allowed during the search.
    /// </summary>
    public RelayCommand IncreaseMaximumFileSizeCommand { get; }

    /// <summary>
    ///     Gets the command to open the specified file path in Explorer.
    /// </summary>
    public RelayCommand<string> OpenInExplorerCommand { get; }

    /// <summary>
    ///     Opens the file explorer and selects the file at the given path.
    /// </summary>
    /// <param name="path">The path of the file or folder to select.</param>
    private static void OpenInExplorer(string path)
    {
        var processStartInfo = new ProcessStartInfo("explorer.exe", $"/select,\"{path}\"")
        {
            UseShellExecute = true
        };
        using var process = Process.Start(processStartInfo);
    }

    /// <summary>
    ///     Starts a scanning process asynchronously with the given parameters.
    /// </summary>
    private async Task RunScanAsync()
    {
        if (SteamIDValidator.IsSteamID64(ScanQuery) is false)
        {
            _notificationService.ShowNotification("This doesn't look like a SteamID64, please try again.");
            ScanQuery = string.Empty;
            return;
        }

        _isScanning = true;
        var start = Stopwatch.GetTimestamp();

        var extensions = _useSelectedExtensions ? GetSelectedExtensions() : Array.Empty<string>();
        var size = _limitFileSize ? ByteConverter.ConvertFromMegabytes(_maxFileSizeInMb) : 0;
        var steamID64 = new SteamID64(long.Parse(ScanQuery));
        var scanningService = _serviceProvider.GetRequiredService<IScanningService>();

        try
        {
            _notificationService.ShowNotification("Keep your eyes open, scanning starts now!");

            var result = await scanningService.StartScanning(steamID64, _limitFileSize, size, _useSelectedExtensions,
                _cancellationTokenSource.Token, extensions).ConfigureAwait(false);
            MatchingFiles = new ObservableCollection<string>(result.GetResultSortedByLength());

            _notificationService.ShowNotification(
                $"We're done scanning! It took {Stopwatch.GetElapsedTime(start).TotalSeconds:F1} seconds to scan {result.TotalScannedFiles} out of {result.TotalFiles} files!");
        }
        catch (TaskCanceledException)
        {
            _notificationService.ShowNotification("Scanning has been cancelled.");
        }
        finally
        {
            _isScanning = false;
        }
    }

    /// <summary>
    ///     Cancels the current scanning process, if there is one.
    /// </summary>
    private void CancelScan()
    {
        if (_isScanning is false)
        {
            _notificationService.ShowNotification("Nothing to cancel - there are no active scanning processes.");
            return;
        }

        _cancellationTokenSource?.Cancel();
        _cancellationTokenSource = new CancellationTokenSource();
        _notificationService.ShowNotification("Cancelling scan. Please wait...");
    }

    /// <summary>
    ///     Returns an array of file extensions that have been selected for scanning.
    /// </summary>
    /// <returns>An array of selected file extensions in the format "*.[extension]".</returns>
    private string[] GetSelectedExtensions()
    {
        return (from extension in _availableExtensions where extension.Selected select "*" + extension.Extension)
            .ToArray();
    }

    /// <summary>
    ///     Changes the selected state of all available extensions to the given state.
    /// </summary>
    private void ChangeSelected(bool check)
    {
        foreach (var item in _availableExtensions.Where(x => x.Selected != check).ToList()) item.Selected = check;
        FilteredCollectionViewSource.Refresh();
    }

    /// <summary>
    ///     Loads the search extensions asynchronously and updates the collection view source.
    /// </summary>
    private async void LoadSearchExtensionsAsync()
    {
        var hashSet = await _steamClient.GetFileExtensionsAsync();
        var searchExtensions = hashSet.Where(x => string.IsNullOrWhiteSpace(x) is false)
            .Select(x => new SearchExtension(x)).ToList();
        var observableSearchExtensions =
            new ObservableCollection<SearchExtension>(searchExtensions.OrderBy(x => x.Extension.Length));
        _availableExtensions = observableSearchExtensions;
        await UpdateCollectionViewSource();
    }

    /// <summary>
    ///     Updates the collection view source with the available search extensions.
    /// </summary>
    private async Task UpdateCollectionViewSource()
    {
        await Application.Current.Dispatcher.InvokeAsync(() =>
        {
            _filteredCollectionViewSource.Source = _availableExtensions;
            _filteredCollectionViewSource.Filter += FilterSearchExtensions;
            OnPropertyChanged(nameof(FilteredCollectionViewSource));
        });
    }

    /// <summary>
    ///     Filters the search extensions based on a query string.
    /// </summary>
    private void FilterSearchExtensions(object sender, FilterEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(ExtensionQuery))
        {
            e.Accepted = true;
            return;
        }

        if (e.Item is SearchExtension searchExtension &&
            searchExtension.Extension.Contains(ExtensionQuery, StringComparison.OrdinalIgnoreCase))
        {
            e.Accepted = true;
            return;
        }

        e.Accepted = false;
    }
}