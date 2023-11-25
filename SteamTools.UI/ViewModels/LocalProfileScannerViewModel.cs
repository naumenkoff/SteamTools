using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using SteamTools.ProfileScanner.Abstractions;

namespace SteamTools.UI.ViewModels;

public class LocalProfileScannerViewModel : ObservableObject
{
    private readonly IProfileScannerService _profileScannerService;

    public LocalProfileScannerViewModel(IProfileScannerService profileScannerService)
    {
        _profileScannerService = profileScannerService;
        LocalProfiles = new ObservableCollection<LocalProfile>();

        LoadLocalProfiles();
    }

    public ObservableCollection<LocalProfile> LocalProfiles { get; }

    private async void LoadLocalProfiles() // skipcq: CS-R1005
    {
        var profiles = await _profileScannerService.GetProfiles();
        foreach (var profile in profiles) LocalProfiles.Add(profile);
    }
}