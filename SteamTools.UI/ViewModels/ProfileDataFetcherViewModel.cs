using System;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using SteamTools.ProfileDataFetcher.Services;

namespace SteamTools.UI.ViewModels;

public class ProfileDataFetcherViewModel : ObservableObject
{
    private readonly IServiceProvider _serviceProvider;

    private string _enteredText;

    public ProfileDataFetcherViewModel(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        GetProfileDetailsCommand = new RelayCommand(GetProfileDetails);
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
        var summaries = profile.GetProfileSummaries();

        MessageBox.Show(summaries);
    }
}