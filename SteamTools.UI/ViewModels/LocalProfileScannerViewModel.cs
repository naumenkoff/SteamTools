using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using SteamTools.Domain.Models;

namespace SteamTools.UI.ViewModels;

public class LocalProfileScannerViewModel : ObservableObject
{
    private readonly ILocalProfileStorage _localProfileStorage;

    public LocalProfileScannerViewModel(ILocalProfileStorage localProfileStorage)
    {
        _localProfileStorage = localProfileStorage;
        LocalProfiles = new ObservableCollection<LocalProfile>();

        LoadSearchExtensionsAsync();
    }

    public ObservableCollection<LocalProfile> LocalProfiles { get; }

    private async void LoadSearchExtensionsAsync() // skipcq: CS-R1005
    {
        await _localProfileStorage.InitializeAsync();
        foreach (var account in _localProfileStorage.Accounts) LocalProfiles.Add(account);
    }
}