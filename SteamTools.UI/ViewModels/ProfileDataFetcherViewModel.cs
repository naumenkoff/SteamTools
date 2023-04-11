using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using SteamTools.Core.Enums;
using SteamTools.Core.Services;
using SteamTools.ProfileDataFetcher.Models;
using SteamTools.ProfileDataFetcher.Services;

namespace SteamTools.UI.ViewModels;

public class ProfileDataFetcherViewModel : ObservableObject
{
    private readonly INotificationService _notificationService;
    private readonly IServiceProvider _serviceProvider;
    private SteamProfile _currentSteamProfile;
    private string _enteredText;
    private ObservableCollection<SteamProfile> _steamProfiles;

    public ProfileDataFetcherViewModel(IServiceProvider serviceProvider, INotificationService notificationService)
    {
        _serviceProvider = serviceProvider;
        GetProfileDetailsCommand = new AsyncRelayCommand(GetProfileDetails);
        CopyToClipboardCommand = new RelayCommand<object>(CopyText);
        OpenInBrowserCommand = new RelayCommand<object>(OpenInBrowser);
        SelectProfileFromHistoryCommand = new RelayCommand<object>(SelectProfileFromHistory);

        CurrentSteamProfile = SteamProfile.Empty;
        SteamProfiles = new ObservableCollection<SteamProfile>();
        _notificationService = notificationService;
    }

    public ObservableCollection<SteamProfile> SteamProfiles
    {
        get => _steamProfiles;
        set
        {
            _steamProfiles = value;
            OnPropertyChanged();
        }
    }

    public SteamProfile CurrentSteamProfile
    {
        get => _currentSteamProfile;
        set
        {
            _currentSteamProfile = value;
            OnPropertyChanged();
        }
    }

    public RelayCommand<object> OpenInBrowserCommand { get; }
    public AsyncRelayCommand GetProfileDetailsCommand { get; }
    public RelayCommand<object> CopyToClipboardCommand { get; }
    public RelayCommand<object> SelectProfileFromHistoryCommand { get; }

    public string EnteredText
    {
        get => _enteredText;
        set
        {
            _enteredText = value;
            OnPropertyChanged();
        }
    }

    private void CopyText(object parameter)
    {
        var text = parameter.ToString();
        if (string.IsNullOrEmpty(text)) return;

        Clipboard.SetText(text);
        _notificationService.ShowNotification("Copied!", NotificationLevel.Common);
    }

    private void OpenInBrowser(object parameter)
    {
        var text = parameter.ToString();
        if (string.IsNullOrEmpty(text)) return;

        Process.Start(new ProcessStartInfo { FileName = text, UseShellExecute = true });
        _notificationService.ShowNotification("Opened in browser!", NotificationLevel.Common);
    }

    private void SelectProfileFromHistory(object parameter)
    {
        if (parameter is not SteamProfile profile) return;
        CurrentSteamProfile = profile;
        EnteredText = profile.Request;
    }

    private async Task GetProfileDetails()
    {
        var start = Stopwatch.GetTimestamp();
        var factory = _serviceProvider.GetRequiredService<ISteamProfileService>();
        var profile = await factory.GetProfileAsync(EnteredText);
        var selected = SelectProfile(profile);
        var responseTime = Stopwatch.GetElapsedTime(start);
        if (selected)
            _notificationService.ShowNotification(
                $"Profile information successfully obtained in {responseTime.TotalMilliseconds} ms!",
                NotificationLevel.Common);
    }

    private bool SelectProfile(SteamProfile steamProfile)
    {
        if (steamProfile.IsEmpty())
        {
            CurrentSteamProfile = SteamProfile.Empty;
            return false;
        }

        var existingProfile = SteamProfiles.FirstOrDefault(x => x.SteamID32.ID32 == steamProfile.SteamID32.ID32);
        if (existingProfile is not null)
        {
            var index = SteamProfiles.IndexOf(existingProfile);
            SteamProfiles.Move(index, 0);
            CurrentSteamProfile = existingProfile;
        }
        else
        {
            SteamProfiles.Insert(0, steamProfile);
            CurrentSteamProfile = steamProfile;

            if (SteamProfiles.Count > 4)
            {
                var lastItem = SteamProfiles.Last();
                SteamProfiles.Remove(lastItem);
            }
        }

        return true;
    }
}