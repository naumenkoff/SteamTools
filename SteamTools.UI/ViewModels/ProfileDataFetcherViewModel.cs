using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using SteamTools.ProfileDataFetcher.Models;
using SteamTools.ProfileDataFetcher.Services;

namespace SteamTools.UI.ViewModels;

public class ProfileDataFetcherViewModel : ObservableObject
{
    private readonly IServiceProvider _serviceProvider;
    private string _enteredText;
    private TimeSpan _responseTime;
    private SteamProfile _steamCurrentSteamProfile;
    private ObservableCollection<SteamProfile> _steamProfiles;

    public ProfileDataFetcherViewModel(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        GetProfileDetailsCommand = new RelayCommand(GetProfileDetails);
        CopyToClipboardCommand = new RelayCommand<object>(CopyText);
        OpenInBrowserCommand = new RelayCommand<object>(OpenInBrowser);
        SelectProfileFromHistoryCommand = new RelayCommand<object>(SelectProfileFromHistory);

        CurrentSteamProfile = SteamProfile.Empty;
        SteamProfiles = new ObservableCollection<SteamProfile>();
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
        get => _steamCurrentSteamProfile;
        set
        {
            _steamCurrentSteamProfile = value;
            OnPropertyChanged();
        }
    }

    public RelayCommand<object> OpenInBrowserCommand { get; }
    public RelayCommand GetProfileDetailsCommand { get; }
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

    public TimeSpan ResponseTime
    {
        get => _responseTime;
        set
        {
            _responseTime = value;
            OnPropertyChanged();
        }
    }

    private static void CopyText(object parameter)
    {
        var text = parameter.ToString();
        if (string.IsNullOrEmpty(text)) return;

        Clipboard.SetText(text);
    }

    private static void OpenInBrowser(object parameter)
    {
        var text = parameter.ToString();
        if (string.IsNullOrEmpty(text)) return;

        Process.Start(new ProcessStartInfo { FileName = text, UseShellExecute = true });
    }

    private void SelectProfileFromHistory(object parameter)
    {
        if (parameter is not SteamProfile profile) return;
        CurrentSteamProfile = profile;
        EnteredText = profile.Request;
    }

    private async void GetProfileDetails()
    {
        var start = Stopwatch.GetTimestamp();
        var factory = _serviceProvider.GetRequiredService<ISteamProfileService>();
        var profile = await factory.GetProfileAsync(EnteredText);
        CurrentSteamProfile = profile;
        AddProfile(profile);
        ResponseTime = Stopwatch.GetElapsedTime(start);
    }

    private void AddProfile(SteamProfile steamProfile)
    {
        var existingProfile = SteamProfiles.FirstOrDefault(x => x.SteamID32.ID32 == steamProfile.SteamID32.ID32);
        if (existingProfile is not null)
        {
            var index = SteamProfiles.IndexOf(existingProfile);
            SteamProfiles.Move(index, 0);
        }
        else
        {
            SteamProfiles.Insert(0, steamProfile);
            if (SteamProfiles.Count > 4)
            {
                var lastItem = SteamProfiles.Last();
                SteamProfiles.Remove(lastItem);
            }
        }
    }
}