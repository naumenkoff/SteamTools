using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using SteamTools.Core.Services;
using SteamTools.ProfileDataFetcher.Models;
using SteamTools.ProfileDataFetcher.Services.Interfaces;

namespace SteamTools.UI.ViewModels;

public class ProfileDataFetcherViewModel : ObservableObject
{
    private readonly INotificationService _notificationService;
    private readonly IServiceProvider _serviceProvider;
    private readonly ObservableCollection<SteamProfile> _steamProfiles;
    private string _cachedText;
    private SteamProfile _currentSteamProfile;
    private bool _showGrid;

    public ProfileDataFetcherViewModel(IServiceProvider serviceProvider, INotificationService notificationService)
    {
        _serviceProvider = serviceProvider;
        _notificationService = notificationService;

        CurrentSteamProfile = SteamProfile.Empty;
        SteamProfiles = new ObservableCollection<SteamProfile>();

        GetProfileDetailsCommand = new AsyncRelayCommand<string>(GetSteamProfileAsync);
        CopyToClipboardCommand = new RelayCommand<object>(CopyText);
        OpenInBrowserCommand = new RelayCommand<object>(OpenInBrowser);
        SelectProfileFromHistoryCommand = new AsyncRelayCommand<object>(SelectProfileFromHistoryAsync);
        ResetSelectedProfileCommand = new RelayCommand(ResetSelectedProfile);
    }

    public ObservableCollection<SteamProfile> SteamProfiles
    {
        get => _steamProfiles;
        private init
        {
            _steamProfiles = value;
            OnPropertyChanged();
        }
    }

    public SteamProfile CurrentSteamProfile
    {
        get => _currentSteamProfile;
        private set
        {
            ShowGrid = string.IsNullOrWhiteSpace(value.SteamID) is false;
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

    public RelayCommand<object> OpenInBrowserCommand { get; }
    public AsyncRelayCommand<string> GetProfileDetailsCommand { get; }
    public RelayCommand<object> CopyToClipboardCommand { get; }
    public RelayCommand ResetSelectedProfileCommand { get; }
    public AsyncRelayCommand<object> SelectProfileFromHistoryCommand { get; }

    private void ResetSelectedProfile()
    {
        CurrentSteamProfile = SteamProfile.Empty;
    }

    private void CopyText(object parameter)
    {
        var text = parameter.ToString();
        if (string.IsNullOrWhiteSpace(text)) return;
        try
        {
            Clipboard.SetText(text);
            _notificationService.RegisterNotification("Text copied like a boss! Let's paste it where it belongs");
        }
        catch
        {
            _notificationService.RegisterNotification("System clipboard is unavailable");
        }
    }

    private void OpenInBrowser(object parameter)
    {
        var text = parameter.ToString();
        if (string.IsNullOrWhiteSpace(text)) return;

        var processStartInfo = new ProcessStartInfo { FileName = text, UseShellExecute = true };
        using var process = Process.Start(processStartInfo);

        _notificationService.RegisterNotification("Time to open up that browser and see what we've got! Let's gooo!");
    }

    private async Task SelectProfileFromHistoryAsync(object parameter)
    {
        if (parameter is not SteamProfile steamProfile) return;
        await GetSteamProfileAsync(steamProfile.SteamID64.AsString);
    }

    private async Task GetSteamProfileAsync(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            ResetSelectedProfile();
            return;
        }

        var start = Stopwatch.GetTimestamp();
        _notificationService.RegisterNotification("Hold tight, we're on the prowl for your profile!");

        var factory = _serviceProvider.GetRequiredService<ISteamProfileService>();
        var profile = await factory.GetProfileAsync(text);

        if (profile.IsEmpty)
        {
            _notificationService.RegisterNotification("Uh-oh, looks like this profile needs a bit of filling up!");
            return;
        }

        SelectSteamProfile(profile);
        _notificationService.RegisterNotification(
            $"Tada! Your profile has been found in just {Stopwatch.GetElapsedTime(start).TotalSeconds:F1} sec. flat!");
    }

    private void SelectSteamProfile(SteamProfile steamProfile)
    {
        if (steamProfile.IsEmpty)
        {
            ResetSelectedProfile();
            return;
        }

        var existingProfile = SteamProfiles.FirstOrDefault(x => x.SteamID32.AsUInt == steamProfile.SteamID32.AsUInt);
        if (existingProfile?.IsEmpty is false) SelectExistingSteamProfile(existingProfile);
        else SelectNewSteamProfile(steamProfile);
    }

    private void SelectExistingSteamProfile(SteamProfile steamProfile)
    {
        var oldIndex = SteamProfiles.IndexOf(steamProfile);
        SteamProfiles.Move(oldIndex, 0);
        CurrentSteamProfile = steamProfile;
    }

    private void SelectNewSteamProfile(SteamProfile steamProfile)
    {
        SteamProfiles.Insert(0, steamProfile);
        CurrentSteamProfile = steamProfile;
        RemoveLastSteamProfile();
    }

    private void RemoveLastSteamProfile()
    {
        if (SteamProfiles.Count <= 4) return;
        var steamProfile = SteamProfiles.Last();
        SteamProfiles.Remove(steamProfile);
    }
}