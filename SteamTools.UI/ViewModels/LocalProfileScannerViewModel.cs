using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using SteamTools.LocalProfileScanner;
using SteamTools.LocalProfileScanner.Models;
using SteamTools.LocalProfileScanner.Services;

namespace SteamTools.UI.ViewModels;

public class LocalProfileScannerViewModel : ObservableObject
{
    private readonly LocalProfileStorage _localProfileStorage;
    private readonly ProfileScannerService _scannerService;

    public LocalProfileScannerViewModel(ProfileScannerService scannerService, LocalProfileStorage localProfileStorage)
    {
        LocalProfiles = new ObservableCollection<ILocalProfile>();
        _localProfileStorage = localProfileStorage;
        _scannerService = scannerService;
        LoadSearchExtensionsAsync();
    }

    public ObservableCollection<ILocalProfile> LocalProfiles { get; }

    private async void LoadSearchExtensionsAsync() // skipcq: CS-R1005
    {
        await _scannerService.ExecuteAsync();
        foreach (var account in _localProfileStorage.Accounts) LocalProfiles.Add(account);
    }
}