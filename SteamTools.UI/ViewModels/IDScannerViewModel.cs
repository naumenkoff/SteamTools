using System;
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
using SteamTools.Core.Enums;
using SteamTools.Core.Models;
using SteamTools.Core.Services;
using SteamTools.Core.Utilities;
using SteamTools.IDScanner.Services;
using SteamTools.UI.Models;
using ByteConverter = SteamTools.Core.Utilities.ByteConverter;

namespace SteamTools.UI.ViewModels;

public class IDScannerViewModel : ObservableObject
{
    private readonly CollectionViewSource _collectionViewSource;
    private readonly INotificationService _notificationService;
    private readonly IScanningService _scanningService;
    private readonly ISteamClient _steamClient;
    private CancellationTokenSource _cancellationTokenSource;
    private string _filterSearchQuery;
    private ObservableCollection<string> _foundProfiles;
    private bool _isLimitMaximumFileSizeEnabled;
    private bool _isRunning;
    private int _maximumFileSizeInMegaBytes;
    private string _scanQuery;
    private ObservableCollection<SearchExtension> _searchExtensions;
    private bool _useExtensions;

    public IDScannerViewModel(ISteamClient steamClient, IScanningService scanningService,
        INotificationService notificationService)
    {
        DecreaseMaximumFileSizeCommand = new RelayCommand(() => MaximumFileSizeInMegaBytes--);
        IncreaseMaximumFileSizeCommand = new RelayCommand(() => MaximumFileSizeInMegaBytes++);
        SelectAllSearchExtensionsCommand = new RelayCommand(() => ChangeSelected(true));
        ResetAllSearchExtensionsCommand = new RelayCommand(() => ChangeSelected(false));
        RunScanCommand = new AsyncRelayCommand(RunScan);
        CancelSearchCommand = new RelayCommand(CancelSearch);
        OpenInExplorerCommand = new RelayCommand<string>(OpenInExplorer);

        _steamClient = steamClient;
        _collectionViewSource = new CollectionViewSource();
        _searchExtensions = new ObservableCollection<SearchExtension>();
        _foundProfiles = new ObservableCollection<string>();
        _scanningService = scanningService;
        _notificationService = notificationService;
        _cancellationTokenSource = new CancellationTokenSource();
    }

    public AsyncRelayCommand RunScanCommand { get; }
    public RelayCommand SelectAllSearchExtensionsCommand { get; }
    public RelayCommand ResetAllSearchExtensionsCommand { get; }
    public RelayCommand DecreaseMaximumFileSizeCommand { get; }
    public RelayCommand IncreaseMaximumFileSizeCommand { get; }
    public RelayCommand<string> OpenInExplorerCommand { get; }

    public string ScanQuery
    {
        get => _scanQuery;
        set
        {
            _scanQuery = value;
            OnPropertyChanged();
        }
    }

    public string FilterSearchQuery
    {
        get => _filterSearchQuery;
        set
        {
            _filterSearchQuery = value;
            OnPropertyChanged();
            _collectionViewSource.View.Refresh();
        }
    }

    public ICollectionView FilteredSearchExtensions => _collectionViewSource.View;

    public bool IsLimitMaximumFileSizeEnabled
    {
        get => _isLimitMaximumFileSizeEnabled;
        set
        {
            _isLimitMaximumFileSizeEnabled = value;
            OnPropertyChanged();
        }
    }

    public int MaximumFileSizeInMegaBytes
    {
        get => _maximumFileSizeInMegaBytes;
        set
        {
            _maximumFileSizeInMegaBytes = Math.Min(Math.Max(0, value), 1024);
            OnPropertyChanged();
        }
    }

    public ObservableCollection<string> FoundProfiles
    {
        get => _foundProfiles;
        set
        {
            _foundProfiles = value;
            OnPropertyChanged();
        }
    }

    public bool UseExtensions
    {
        get => _useExtensions;
        set
        {
            _useExtensions = value;
            OnPropertyChanged();
        }
    }

    public RelayCommand CancelSearchCommand { get; }

    private static void OpenInExplorer(string path)
    {
        Process.Start("explorer.exe", $"/select,\"{path}\"");
    }

    private async Task RunScan()
    {
        if (ScanQuery.All(char.IsDigit) is false || SteamIDValidator.IsSteamID64(ScanQuery) is false)
        {
            ScanQuery = string.Empty;
            return;
        }

        var start = Stopwatch.GetTimestamp();
        var extensions = _useExtensions ? GetSelectedExtensions() : Array.Empty<string>();
        var size = _isLimitMaximumFileSizeEnabled ? ByteConverter.ConvertFromMegabytes(_maximumFileSizeInMegaBytes) : 0;
        var steamID64 = new SteamID64(long.Parse(ScanQuery));
        try
        {
            _isRunning = true;
            var result = await _scanningService.StartScanning(steamID64, _isLimitMaximumFileSizeEnabled, size,
                _useExtensions,
                _cancellationTokenSource.Token, extensions);
            FoundProfiles = new ObservableCollection<string>(result);
            _notificationService.ShowNotification(
                $"Elapsed {Stopwatch.GetElapsedTime(start).TotalSeconds} sec, scanned {_scanningService.ScannedFileCount} out of {_scanningService.TotalFileCount} files!",
                NotificationLevel.Common);
            _isRunning = false;
        }
        catch (TaskCanceledException)
        {
            _notificationService.ShowNotification("Search was canceled!", NotificationLevel.Common);
            _isRunning = false;
            GC.Collect();
        }
    }

    private void CancelSearch()
    {
        if (_isRunning is false)
        {
            _notificationService.ShowNotification("There's no running scans", NotificationLevel.Common);
            return;
        }

        _cancellationTokenSource?.Cancel();
        _cancellationTokenSource = new CancellationTokenSource();
        _notificationService.ShowNotification("Attempt to cancel search", NotificationLevel.Common);
    }

    private string[] GetSelectedExtensions()
    {
        return (from extension in _searchExtensions where extension.Selected select "*" + extension.Extension)
            .ToArray();
    }

    private void ChangeSelected(bool check)
    {
        foreach (var item in _searchExtensions.Where(x => x.Selected == !check).ToList()) item.Selected = check;
        _collectionViewSource.View.Refresh();
    }

    public async Task InitializeAsync()
    {
        await LoadSearchExtensionsAsync();
    }

    private async Task LoadSearchExtensionsAsync()
    {
        var hashSet = await _steamClient.GetExtensionsAsync();
        var searchExtensions = hashSet.Where(x => string.IsNullOrWhiteSpace(x) is false)
            .Select(x => new SearchExtension(x)).ToList();
        var observableSearchExtensions =
            new ObservableCollection<SearchExtension>(searchExtensions.OrderBy(x => x.Extension.Length));
        _searchExtensions = observableSearchExtensions;
        await UpdateCollectionViewSource();
    }

    private async Task UpdateCollectionViewSource()
    {
        await Application.Current.Dispatcher.InvokeAsync(() =>
        {
            _collectionViewSource.Source = _searchExtensions;
            _collectionViewSource.Filter += FilterSearchExtensions;
            OnPropertyChanged(nameof(FilteredSearchExtensions));
        });
    }

    private void FilterSearchExtensions(object sender, FilterEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(FilterSearchQuery))
        {
            e.Accepted = true;
            return;
        }

        if (e.Item is SearchExtension searchExtension &&
            searchExtension.Extension.Contains(FilterSearchQuery, StringComparison.OrdinalIgnoreCase))
        {
            e.Accepted = true;
            return;
        }

        e.Accepted = false;
    }
}