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
using SteamTools.ProfileDataFetcher.Services;

namespace SteamTools.UI.ViewModels;

public class ProfileDataFetcherViewModel : ObservableObject
{
    private readonly INotificationService _notificationService;
    private readonly IServiceProvider _serviceProvider;
    private readonly ObservableCollection<SteamProfile> _steamProfiles;
    private SteamProfile _currentSteamProfile;
    private string _enteredText;

    private bool _showGrid;

    public ProfileDataFetcherViewModel(IServiceProvider serviceProvider, INotificationService notificationService)
    {
        _serviceProvider = serviceProvider;
        _notificationService = notificationService;

        CurrentSteamProfile = SteamProfile.Empty;
        SteamProfiles = new ObservableCollection<SteamProfile>();

        GetProfileDetailsCommand = new AsyncRelayCommand(RetrieveProfileInformationWithNotificationAsync);
        CopyToClipboardCommand = new RelayCommand<object>(CopyText);
        OpenInBrowserCommand = new RelayCommand<object>(OpenInBrowser);
        SelectProfileFromHistoryCommand = new AsyncRelayCommand<object>(SelectProfileFromHistoryAsync);
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

    public string EnteredText
    {
        get => _enteredText;
        set
        {
            if (_enteredText == value) return;

            _enteredText = value;
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

    public RelayCommand<object> OpenInBrowserCommand { get; }
    public AsyncRelayCommand GetProfileDetailsCommand { get; }
    public RelayCommand<object> CopyToClipboardCommand { get; }
    public AsyncRelayCommand<object> SelectProfileFromHistoryCommand { get; }

    private void CopyText(object parameter)
    {
        var text = parameter.ToString();
        if (string.IsNullOrWhiteSpace(text)) return;

        Clipboard.SetText(text);
        _notificationService.ShowNotification("Copied!");
    }

    private void OpenInBrowser(object parameter)
    {
        var text = parameter.ToString();
        if (string.IsNullOrWhiteSpace(text)) return;

        var processStartInfo = new ProcessStartInfo { FileName = text, UseShellExecute = true };
        using var process = Process.Start(processStartInfo);

        _notificationService.ShowNotification("Opened in the browser!");
    }

    private async Task SelectProfileFromHistoryAsync(object parameter)
    {
        if (parameter is not SteamProfile steamProfile) return;
        EnteredText = steamProfile.Request;
        await RetrieveProfileInformationWithNotificationAsync();
    }

    // Click or Enter => RetrieveProfileInformationWithNotification
    // RetrieveProfileInformationWithNotification => RetrieveProfileInformation
    // RetrieveProfileInformation => SelectProfile
    // SelectProfile => SelectExistingProfile, SelectNewProfile
    private async Task RetrieveProfileInformationWithNotificationAsync()
    {
        var start = Stopwatch.GetTimestamp();
        var selected = await RetrieveProfileInformationAsync();
        var responseTime = Stopwatch.GetElapsedTime(start);
        var notification = selected
            ? $"Found your profile in {responseTime.TotalSeconds:F1} seconds!"
            : "Selection cleared!";
        _notificationService.ShowNotification(notification);
    }

    private async Task<bool> RetrieveProfileInformationAsync()
    {
        _notificationService.ShowNotification("Searching for your profile...");
        var factory = _serviceProvider.GetRequiredService<ISteamProfileService>();
        var profile = await factory.GetProfileAsync(EnteredText);
        return SelectProfile(profile);
    }

    private bool SelectProfile(SteamProfile steamProfile)
    {
        if (steamProfile.IsEmpty)
        {
            CurrentSteamProfile = SteamProfile.Empty;
            return false;
        }

        if (SelectExistingProfile(steamProfile) is false) SelectNewProfile(steamProfile);
        return true;
    }

    private bool SelectExistingProfile(SteamProfile newProfile)
    {
        var existingProfile = SteamProfiles.FirstOrDefault(x => x.SteamID32.AsUInt == newProfile.SteamID32.AsUInt);
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