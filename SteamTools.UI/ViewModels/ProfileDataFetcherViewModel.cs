using System;
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
    private SteamProfile _steamProfile;

    public ProfileDataFetcherViewModel(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        GetProfileDetailsCommand = new RelayCommand(GetProfileDetails);
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

    public RelayCommand GetProfileDetailsCommand { get; }

    public string EnteredText
    {
        get => _enteredText;
        set
        {
            _enteredText = value;
            OnPropertyChanged();
        }
    }

    private async void GetProfileDetails()
    {
        var factory = _serviceProvider.GetRequiredService<ISteamProfileBuilder>();
        var profile = await factory.BuildSteamProfileAsync(EnteredText);
        ProfileDetails = profile;
    }
}