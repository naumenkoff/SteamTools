using System;
using System.Diagnostics;
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
    private SteamProfile _steamProfile;

    public ProfileDataFetcherViewModel(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        GetProfileDetailsCommand = new RelayCommand(GetProfileDetails);
        CopyToClipboardCommand = new RelayCommand<object>(CopyText);
        OpenInBrowserCommand = new RelayCommand<object>(OpenInBrowser);
        ProfileDetails = SteamProfile.Empty;
    }

    public SteamProfile ProfileDetails
    {
        get => _steamProfile;
        set
        {
            _steamProfile = value;
            OnPropertyChanged();
        }
    }

    public RelayCommand<object> OpenInBrowserCommand { get; }
    public RelayCommand GetProfileDetailsCommand { get; }
    public RelayCommand<object> CopyToClipboardCommand { get; }

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

    private void CopyText(object parameter)
    {
        var text = parameter.ToString();
        if (string.IsNullOrEmpty(text)) return;

        Clipboard.SetText(text);
    }

    private void OpenInBrowser(object parameter)
    {
        var text = parameter.ToString();
        if (string.IsNullOrEmpty(text)) return;

        Process.Start(new ProcessStartInfo { FileName = text, UseShellExecute = true });
    }

    private async void GetProfileDetails()
    {
        var start = Stopwatch.GetTimestamp();
        var factory = _serviceProvider.GetRequiredService<ISteamProfileService>();
        var profile = await factory.GetProfileAsync(EnteredText);
        ProfileDetails = profile;
        ResponseTime = Stopwatch.GetElapsedTime(start);
    }
}