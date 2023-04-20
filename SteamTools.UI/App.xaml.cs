﻿using System;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SteamTools.Core.Models.Steam;
using SteamTools.Core.Services;
using SteamTools.IDScanner.Models;
using SteamTools.IDScanner.Services;
using SteamTools.LocalProfileScanner;
using SteamTools.LocalProfileScanner.Services;
using SteamTools.ProfileDataFetcher.Clients;
using SteamTools.ProfileDataFetcher.Providers;
using SteamTools.ProfileDataFetcher.Services;
using SteamTools.UI.Services.Navigation;
using SteamTools.UI.Services.Notifications;
using SteamTools.UI.ViewModels;
using SteamTools.UI.Views;

namespace SteamTools.UI;

/// <summary>
///     Interaction logic for App.xaml
/// </summary>
public partial class App
{
    private readonly IConfiguration _configuration;
    private readonly IServiceProvider _serviceProvider;

    public App()
    {
        _configuration = BuildConfiguration();
        _serviceProvider = ConfigureServices();
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        var main = _serviceProvider.GetRequiredService<MainWindow>();
        var navigator = _serviceProvider.GetRequiredService<INavigationService>();
        navigator.Navigate<ProfileDataFetcherViewModel>();
        main.Show();
        base.OnStartup(e);
    }

    private static IConfiguration BuildConfiguration()
    {
        var configuration = new ConfigurationBuilder();

        configuration.AddJsonFile("appsettings.json");

        return configuration.Build();
    }

    private IServiceProvider ConfigureServices()
    {
        var services = new ServiceCollection();

        services.AddSingleton<Func<Type, ObservableObject>>(serviceProvider =>
            viewModelType => (ObservableObject)serviceProvider.GetRequiredService(viewModelType));
        // MainWindowViewModel doesn't have a default constructor, so we are setting the DataContext
        services.AddSingleton(provider => new MainWindow
            { DataContext = provider.GetRequiredService<MainWindowViewModel>() });

        services.AddSingleton<MainWindowViewModel>();
        services.AddSingleton<IDScannerViewModel>();
        services.AddSingleton<LocalProfileScannerViewModel>();
        services.AddSingleton<ProfileDataFetcherViewModel>();
        services.AddSingleton<INavigationService, NavigationService>();
        services.AddSingleton(_configuration);
        services.AddSingleton<ISteamApiKeyProvider, SteamApiKeyProvider>();
        services.AddSingleton<ISteamApiClientCacheService, SteamApiClientCacheService>();
        services.AddSingleton<ISteamProfileRegexProvider, SteamProfileRegexProvider>();
        services.AddSingleton<ISteamClient, SteamClient>();
        services.AddSingleton<INotificationService, PopupNotificationService>();
        services.AddSingleton<LocalProfileStorage>();

        services.AddHttpClient<ISteamApiClient, SteamApiClient>();
        services.AddTransient<ISteamDirectoryFinder, SteamDirectoryFinder>();
        services.AddTransient<ISteamProfileTypeDetector, SteamProfileTypeDetector>();
        services.AddTransient<ISteamProfileService, SteamProfileService>();
        services.AddTransient<ProfileScannerService>();
        services.AddTransient<ScanningResult>();
        services.AddTransient<IScanningService, ScanningService>();

        return services.BuildServiceProvider();
    }
}