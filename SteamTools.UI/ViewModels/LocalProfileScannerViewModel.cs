using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using SteamTools.LocalProfileScanner.Models;

namespace SteamTools.UI.ViewModels;

public class LocalProfileScannerViewModel : ObservableObject
{
    private ObservableCollection<LocalProfile> _localProfiles;

    public LocalProfileScannerViewModel()
    {
        _localProfiles = new ObservableCollection<LocalProfile>(LocalProfile.Accounts);
    }

    public ObservableCollection<LocalProfile> LocalProfiles
    {
        get => _localProfiles;
        set
        {
            _localProfiles = value;
            OnPropertyChanged();
        }
    }
}