using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using SteamTools.ProfileScanner;

namespace SteamTools.UI.ViewModels;

public class LocalProfileScannerViewModel : ObservableObject
{
    #region Private Fields

    private readonly IProfileScannerService _profileScannerService;

    #endregion

    #region Constructor

    public LocalProfileScannerViewModel(IProfileScannerService profileScannerService)
    {
        #region Private Fields

        _profileScannerService = profileScannerService;

        #endregion

        #region Public Properties

        LocalProfiles = [];

        #endregion

        Task.Run(FillProfilesAsync);
    }

    #endregion

    #region Public Properties

    public ObservableCollection<LocalProfile> LocalProfiles { get; }

    #endregion

    #region Private Methods

    private async Task FillProfilesAsync()
    {
        foreach (var item in await _profileScannerService.GetProfiles())
            await Application.Current.Dispatcher.BeginInvoke(() => LocalProfiles.Add(item));
    }

    #endregion

    #region Public Commands

    #endregion
}