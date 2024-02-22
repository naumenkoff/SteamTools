using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SteamTools.Common;
using SteamTools.Presentation.Models;
using SteamTools.ProfileFetcher;

namespace SteamTools.Presentation.ViewModels;

public class ProfileDataFetcherViewModel : ObservableObject
{
    private const int AccountsThatCanBeShow = 4;

    #region Constructor

    public ProfileDataFetcherViewModel(Func<IProfileFetcherService> profileFetcherFactory, INotificationService notificationService)
    {
        #region Private Fields

        _profileFetcherFactory = profileFetcherFactory;
        _notificationService = notificationService;

        #endregion

        #region Public Properties

        CurrentSteamProfile = SteamProfile.Empty;
        SteamProfiles = [];
        ProfileContent = [];

        #endregion

        #region Public Commands

        GetProfileDetailsCommand = new AsyncRelayCommand<string>(FindAccountAsync);
        CopyToClipboardCommand = new RelayCommand<object>(CopyText);
        OpenInBrowserCommand = new RelayCommand<object>(OnHyperLinkClicked);
        SelectProfileFromHistoryCommand = new AsyncRelayCommand<object>(FindAccountFromHistoryAsync);
        ResetSelectedProfileCommand = new RelayCommand(ResetSelection);

        #endregion
    }

    #endregion

    #region Private Fields

    private readonly INotificationService _notificationService;
    private readonly Func<IProfileFetcherService> _profileFetcherFactory;
    private string _cachedText;
    private SteamProfile _currentSteamProfile;
    private bool _showGrid;

    #endregion

    #region Private Methods

    private void CopyText(object parameter)
    {
        var text = parameter.ToString();
        if (string.IsNullOrWhiteSpace(text)) return;

        try
        {
            Clipboard.SetText(text);
            _notificationService.RegisterNotification("Copied");
        }
        catch { _notificationService.RegisterNotification("System clipboard is unavailable"); }
    }

    private void OnHyperLinkClicked(object parameter)
    {
        var text = parameter.ToString();
        if (string.IsNullOrWhiteSpace(text)) return;

        var processStartInfo = new ProcessStartInfo
        {
            FileName = text,
            UseShellExecute = true
        };
        using var process = Process.Start(processStartInfo);

        _notificationService.RegisterNotification("Time to open up that browser and see what we've got! Let's gooo!");
    }

    private async Task FindAccountAsync(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            ResetSelection();
            return;
        }

        var start = Stopwatch.GetTimestamp();
        _notificationService.RegisterNotification("Hold tight, we're on the prowl for your profile!");

        var factory = _profileFetcherFactory();
        var profile = await factory.GetProfileAsync(text);

        if (!profile.ExistOnline)
        {
            _notificationService.RegisterNotification("Uh-oh, looks like this profile needs a bit of filling up!");
            return;
        }

        Select(profile);
        _notificationService.RegisterNotification(
            $"Tada! Your profile has been found in just {Stopwatch.GetElapsedTime(start).TotalSeconds:F1} sec. flat!");
    }

    private async Task FindAccountFromHistoryAsync(object parameter)
    {
        if (parameter is not SteamProfile steamProfile) return;

        CachedText = steamProfile.Request;
        await FindAccountAsync(steamProfile.ID64.AsString);
    }

    private void ResetSelection()
    {
        CurrentSteamProfile = SteamProfile.Empty;
    }

    private void Select(SteamProfile steamProfile)
    {
        if (!steamProfile.ExistOnline)
        {
            ResetSelection();
            return;
        }

        var existingProfile = SteamProfiles.FirstOrDefault(x => x.ID32.AsUInt == steamProfile.ID32.AsUInt);
        CurrentSteamProfile = existingProfile?.ExistOnline is true ? SelectExisting(existingProfile) : SelectNew(steamProfile);
        FillProfileContent(steamProfile);
    }

    private void FillProfileContent(SteamProfile steamProfile)
    {
        ProfileContent.Clear();

        ProfileContent.Add(new ProfileContent("SteamID", steamProfile.ID, CopyToClipboardCommand));
        ProfileContent.Add(new ProfileContent("SteamID3", steamProfile.ID3, CopyToClipboardCommand));
        ProfileContent.Add(new ProfileContent("SteamID64", steamProfile.ID32.AsString, CopyToClipboardCommand));
        ProfileContent.Add(new ProfileContent("SteamID32", steamProfile.ID64.AsString, CopyToClipboardCommand));
        ProfileContent.Add(new ProfileContent("Custom URL", steamProfile.PlayerSummaries?.ProfileUrl, OpenInBrowserCommand));
        ProfileContent.Add(new ProfileContent("Permanent URL", steamProfile.Permalink, CopyToClipboardCommand));

        var timeCreated = steamProfile.PlayerSummaries is null
            ? default
            : DateTimeOffset.FromUnixTimeSeconds(steamProfile.PlayerSummaries.TimeCreated).DateTime.ToLongDateString();
        ProfileContent.Add(new ProfileContent("Created At", timeCreated, CopyToClipboardCommand));
    }

    private SteamProfile SelectExisting(SteamProfile steamProfile)
    {
        var oldIndex = SteamProfiles.IndexOf(steamProfile);
        SteamProfiles.Move(oldIndex, 0);
        return steamProfile;
    }

    private SteamProfile SelectNew(SteamProfile steamProfile)
    {
        SteamProfiles.Insert(0, steamProfile);
        if (SteamProfiles.Count >= AccountsThatCanBeShow) SteamProfiles.RemoveAt(SteamProfiles.Count - 1);

        return steamProfile;
    }

    #endregion

    #region Public Properties

    public ObservableCollection<ProfileContent> ProfileContent { get; }

    public ObservableCollection<SteamProfile> SteamProfiles { get; }

    public SteamProfile CurrentSteamProfile
    {
        get => _currentSteamProfile;
        private set
        {
            if (_currentSteamProfile == value) return;

            ShowGrid = value.ExistOnline;
            _currentSteamProfile = value;
            OnPropertyChanged();
        }
    }

    public bool ShowGrid
    {
        get => _showGrid;
        set
        {
            if (_showGrid == value) return;

            _showGrid = value;
            OnPropertyChanged();
        }
    }

    public string CachedText
    {
        get => _cachedText;
        set
        {
            if (_cachedText == value) return;

            _cachedText = value;
            OnPropertyChanged();
        }
    }

    #endregion

    #region Public Commands

    public RelayCommand<object> OpenInBrowserCommand { get; }
    public AsyncRelayCommand<string> GetProfileDetailsCommand { get; }
    public RelayCommand<object> CopyToClipboardCommand { get; }
    public RelayCommand ResetSelectedProfileCommand { get; }
    public AsyncRelayCommand<object> SelectProfileFromHistoryCommand { get; }

    #endregion
}