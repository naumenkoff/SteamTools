﻿using System;
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
        GetProfileDetailsCommand = new AsyncRelayCommand(RetrieveProfileInformationWithNotification);
        CopyToClipboardCommand = new RelayCommand<object>(CopyText);
        OpenInBrowserCommand = new RelayCommand<object>(OpenInBrowser);
        SelectProfileFromHistoryCommand = new AsyncRelayCommand<object>(SelectProfileFromHistory);

        _currentSteamProfile = SteamProfile.Empty;
        _steamProfiles = new ObservableCollection<SteamProfile>();
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

    public string EnteredText
    {
        get => _enteredText;
        set
        {
            _enteredText = value;
            OnPropertyChanged();
        }
    }

    public RelayCommand<object> OpenInBrowserCommand { get; }
    public AsyncRelayCommand GetProfileDetailsCommand { get; }
    public RelayCommand<object> CopyToClipboardCommand { get; }
    public AsyncRelayCommand<object> SelectProfileFromHistoryCommand { get; }

    private void CopyText(object parameter)
    {
        var text = parameter.ToString();
        if (string.IsNullOrWhiteSpace(text)) return;

        Clipboard.SetText(text);
        _notificationService.ShowNotification("Copied!", NotificationLevel.Common);
    }

    private void OpenInBrowser(object parameter)
    {
        var text = parameter.ToString();
        if (string.IsNullOrWhiteSpace(text)) return;

        Process.Start(new ProcessStartInfo { FileName = text, UseShellExecute = true });
        _notificationService.ShowNotification("Opened in browser!", NotificationLevel.Common);
    }

    private async Task SelectProfileFromHistory(object parameter)
    {
        if (parameter is not SteamProfile steamProfile) return;
        EnteredText = steamProfile.Request;
        await RetrieveProfileInformationWithNotification();
    }

    private async Task RetrieveProfileInformationWithNotification()
    {
        var start = Stopwatch.GetTimestamp();
        var selected = await RetrieveProfileInformation();
        var responseTime = Stopwatch.GetElapsedTime(start);
        var notification = selected
            ? $"Profile information successfully obtained in {responseTime.TotalMilliseconds} ms!"
            : "Selection reset!";
        _notificationService.ShowNotification(notification, NotificationLevel.Common);
    }

    private async Task<bool> RetrieveProfileInformation()
    {
        var factory = _serviceProvider.GetRequiredService<ISteamProfileService>();
        var profile = await factory.GetProfileAsync(EnteredText);
        return SelectProfile(profile);
    }

    private bool SelectProfile(SteamProfile steamProfile)
    {
        if (steamProfile.IsEmpty())
        {
            CurrentSteamProfile = SteamProfile.Empty;
            return false;
        }

        if (SelectExistingProfile(steamProfile) is false) SelectNewProfile(steamProfile);

        return true;
    }

    private bool SelectExistingProfile(SteamProfile newProfile)
    {
        var existingProfile = SteamProfiles.FirstOrDefault(x => x.SteamID32.ID32 == newProfile.SteamID32.ID32);
        if (existingProfile is null) return false;

        var oldIndex = SteamProfiles.IndexOf(existingProfile);
        SteamProfiles.Move(oldIndex, 0);
        CurrentSteamProfile = existingProfile;
        return true;
    }

    private void SelectNewProfile(SteamProfile newProfile)
    {
        SteamProfiles.Insert(0, newProfile);
        CurrentSteamProfile = newProfile;

        if (SteamProfiles.Count <= 4) return;
        var lastItem = SteamProfiles.Last();
        SteamProfiles.Remove(lastItem);
    }
}