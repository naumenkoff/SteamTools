using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Data;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SProject.Steam;
using SteamTools.Domain.Factories;
using SteamTools.Domain.Models;
using SteamTools.Domain.Services;
using SteamTools.UI.Models;

namespace SteamTools.UI.ViewModels;

public class IDScannerViewModel : ObservableObject
{
    private readonly string[] _defaultExtensions = { ".acf", ".vdf", ".txt", ".json" };
    private readonly CollectionViewSource _filteredCollectionViewSource;
    private readonly INotificationService _notificationService;
    private readonly IScanningServiceFactory _scanningServiceFactory;
    private readonly ISteamClient _steamClient;
    private ObservableCollection<SearchExtension> _availableExtensions;
    private CancellationTokenSource _cancellationTokenSource;
    private string _extensionQuery;
    private ObservableCollection<string> _matchingFiles;

    public IDScannerViewModel(ISteamClient steamClient, INotificationService notificationService, ScanningOptions scanningOptions,
        IScanningServiceFactory scanningServiceFactory)
    {
        ScanningOptions = scanningOptions;
        _scanningServiceFactory = scanningServiceFactory;
        _steamClient = steamClient;
        _notificationService = notificationService;
        _cancellationTokenSource = new CancellationTokenSource();

        #region Commands

        DecreaseMaximumFileSizeCommand = new RelayCommand(() => ScanningOptions.MaximumFileSize--);
        IncreaseMaximumFileSizeCommand = new RelayCommand(() => ScanningOptions.MaximumFileSize++);
        SelectAllSearchExtensionsCommand = new AsyncRelayCommand(async () => await ChangeFileExtensionsSelectedState(x => x.Selected == false, true));
        ResetAllSearchExtensionsCommand = new AsyncRelayCommand(async () => await ChangeFileExtensionsSelectedState(x => x.Selected, false));
        SelectDefaultExtensionsCommand =
            new AsyncRelayCommand(async () => await ChangeFileExtensionsSelectedState(x => _defaultExtensions.Contains(x.Extension), true));
        RunScanCommand = new AsyncRelayCommand<string>(RunScanAsync);
        CancelScanCommand = new RelayCommand(CancelScan);
        OpenInExplorerCommand = new RelayCommand<string>(OpenInExplorer);
        ExtensionCheckedCommand = new RelayCommand<SearchExtension>(x => { ScanningOptions.Extensions.Add(x.Extension); });
        ExtensionUncheckedCommand = new RelayCommand<SearchExtension>(x => { ScanningOptions.Extensions.Remove(x.Extension); });

        #endregion

        #region Collections

        MatchingFiles = new ObservableCollection<string>();
        _filteredCollectionViewSource = new CollectionViewSource();
        _filteredCollectionViewSource.Filter += FilterFileExtensions;
        _availableExtensions = new ObservableCollection<SearchExtension>();

        #endregion

        LoadSearchExtensionsAsync();
    }

    public ScanningOptions ScanningOptions { get; }

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
            FilteredCollectionViewSource?.Refresh();
        }
    }

    /// <summary>
    ///     Gets the command to run the scan asynchronously.
    /// </summary>
    public AsyncRelayCommand<string> RunScanCommand { get; }


    public RelayCommand<SearchExtension> ExtensionCheckedCommand { get; }
    public RelayCommand<SearchExtension> ExtensionUncheckedCommand { get; }

    /// <summary>
    ///     Gets the command to cancel the ongoing scan.
    /// </summary>
    public RelayCommand CancelScanCommand { get; }

    /// <summary>
    ///     Gets the command to select all file extensions for the scan.
    /// </summary>
    public AsyncRelayCommand SelectAllSearchExtensionsCommand { get; }

    /// <summary>
    ///     Gets the command to reset all file extensions for the scan.
    /// </summary>
    public AsyncRelayCommand ResetAllSearchExtensionsCommand { get; }

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

    public AsyncRelayCommand SelectDefaultExtensionsCommand { get; }

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
    private async Task RunScanAsync(string query)
    {
        if (SteamIDValidator.IsSteamID64(query) is false)
        {
            _notificationService.RegisterNotification("This doesn't look like a SteamID64, please try again.");
            return;
        }

        ScanningOptions.IsScanning = true;

        var start = Stopwatch.GetTimestamp();
        var steamProfile = new SteamProfile(long.Parse(query));
        var token = _cancellationTokenSource.Token;

        _notificationService.RegisterNotification("Keep your eyes open, scanning starts now!");

        var scanningService = _scanningServiceFactory.Create(steamProfile, token);
        try
        {
            var scanningResult = await scanningService.StartScanningAsync();
            MatchingFiles = new ObservableCollection<string>(scanningResult.GetResultSortedByLength());
            _notificationService.RegisterNotification(
                $"We're done scanning! It took {Stopwatch.GetElapsedTime(start).TotalSeconds:F1} seconds to scan {scanningResult.SuccessfullyScannedFiles} out of {scanningResult.TotalScannedFiles} files!");
        }
        catch (OperationCanceledException) { _notificationService.RegisterNotification("Scanning has been cancelled."); }
        catch (Exception) { _notificationService.RegisterNotification("There's something error while scanning, cancelled."); }
        finally { ScanningOptions.IsScanning = false; }
    }

    /// <summary>
    ///     Cancels the current scanning process, if there is one.
    /// </summary>
    private void CancelScan()
    {
        if (ScanningOptions.IsScanning is false) return;

        _cancellationTokenSource?.Cancel();
        _cancellationTokenSource = new CancellationTokenSource();
        _notificationService.RegisterNotification("Cancelling scan. Please wait...");
    }

    /// <summary>
    ///     Loads the search extensions asynchronously and updates the collection view source.
    /// </summary>
    private void LoadSearchExtensionsAsync() // skipcq: CS-R1005
    {
        var fileExtensions = GetFileExtensions();
        _availableExtensions = new ObservableCollection<SearchExtension>(fileExtensions);
        _filteredCollectionViewSource.Source = _availableExtensions;
        OnPropertyChanged(nameof(FilteredCollectionViewSource));
    }

    private IEnumerable<SearchExtension> GetFileExtensions()
    {
        return _steamClient.SteamLibraries.SelectMany(steamLibrary =>
                steamLibrary.EnumerateFiles("*.*", SearchOption.AllDirectories).Select(file => file.Extension)).Distinct()
            .Select(x => new SearchExtension(x)).OrderBy(x => x.Extension.Length);
    }

    private ValueTask ChangeFileExtensionsSelectedState(Func<SearchExtension, bool> func, bool check)
    {
        foreach (var item in _availableExtensions.Where(func)) item.Selected = check;
        _filteredCollectionViewSource.Source = _availableExtensions;
        FilteredCollectionViewSource?.Refresh();
        return ValueTask.CompletedTask;
    }

    /// <summary>
    ///     Filters the search extensions based on a query string.
    /// </summary>
    private void FilterFileExtensions(object sender, FilterEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(ExtensionQuery))
        {
            e.Accepted = true;
            return;
        }

        e.Accepted = e.Item is SearchExtension searchExtension &&
                     searchExtension.Extension.Contains(ExtensionQuery, StringComparison.OrdinalIgnoreCase);
    }
}