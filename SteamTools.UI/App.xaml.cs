﻿using System;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SteamTools.UI.Services.Navigation;
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
        //_configuration = BuildConfiguration();
        _serviceProvider = BuildServiceProvider();
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        var main = _serviceProvider.GetRequiredService<MainWindow>();
        var navigator = _serviceProvider.GetRequiredService<INavigationService>();
        navigator.Navigate<ProfileDataFetcherViewModel>();
        main.Show();

        base.OnStartup(e);
    }

    private IConfiguration BuildConfiguration()
    {
        var configuration = new ConfigurationBuilder();

        configuration.AddJsonFile("appsettings.json");

        return configuration.Build();
    }

    private IServiceProvider BuildServiceProvider()
    {
        var services = new ServiceCollection();

        services.AddSingleton<Func<Type, ObservableObject>>(serviceProvider =>
            viewModelType => (ObservableObject)serviceProvider.GetRequiredService(viewModelType));
        services.AddSingleton(provider => new MainWindow
            { DataContext = provider.GetRequiredService<MainWindowViewModel>() });
        services.AddSingleton<MainWindowViewModel>();
        services.AddSingleton<IDScannerViewModel>();
        services.AddSingleton<LocalProfileScannerViewModel>();
        services.AddSingleton<ProfileDataFetcherViewModel>();
        services.AddSingleton<INavigationService, NavigationService>();

        return services.BuildServiceProvider();
    }
}