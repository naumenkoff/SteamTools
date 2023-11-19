using System;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SProject.Steam;
using SteamTools.Domain.Enumerations;
using SteamTools.Domain.Models;
using SteamTools.Domain.Providers;
using SteamTools.Domain.Services;
using SteamTools.Infrastructure.Models;
using SteamTools.Infrastructure.Services;
using SteamTools.ProfileScanner;
using SteamTools.ProfileScanner.Abstractions;
using SteamTools.ProfileScanner.Providers;
using SteamTools.ProfileScanner.Services;
using SteamTools.SignatureSearcher.Abstractions;
using SteamTools.SignatureSearcher.Factories;
using SteamTools.UI.Services.Navigation;
using SteamTools.UI.Utilities;
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

        #region UI

        services.AddSingleton<Func<Type, ObservableObject>>(serviceProvider =>
            viewModelType => (ObservableObject) serviceProvider.GetRequiredService(viewModelType));
        services.AddSingleton(provider => new MainWindow
        {
            DataContext = provider.GetRequiredService<MainWindowViewModel>()
        });
        services.AddSingleton<MainWindowViewModel>();
        services.AddSingleton<IDScannerViewModel>();
        services.AddSingleton<LocalProfileScannerViewModel>();
        services.AddSingleton<ProfileDataFetcherViewModel>();
        services.AddSingleton<INavigationService, NavigationService>();
        services.AddSingleton(_configuration);

        #endregion

        #region ProfileDataFetcher

        services.AddSingleton<ISteamApiKeyProvider, SteamApiKeyProvider>();
        services.AddSingleton<ISteamApiClientCacheService, SteamApiClientCacheService>();
        services.AddSingleton<ITemplateProvider<SteamProfileType>, SteamProfileTemplateProvider>();
        services.AddHttpClient<ISteamApiClient, SteamApiClient>();
        services.AddTransient<ISteamProfileService, SteamProfileService>();
        services.AddTransient<ISteamProfileTypeDetector, SteamProfileTypeDetector>();

        #endregion

        #region LocalProfileScanner

        services.AddSingleton<ILocalProfileStorage, LocalProfileStorage>();
        services.AddSingleton<ITemplateProvider<IScanner>, ScannerPatternProvider>();

        services.AddTransient<IProfileScannerService, ProfileScannerService>();
        services.RegisterTransientServices<IScanner>();

        #endregion

        #region IDScanner

        services.AddSingleton<IScanningServiceFactory, ScanningServiceFactory>();

        #endregion

        services.AddSingleton<ScanningOptions>();

        #region Core
        
        services.AddSteamClient();
        services.AddSingleton<SteamClient>();
        
        services.AddSingleton<INotificationService, SimpleNotificationService>();

        #endregion

        return services.BuildServiceProvider();
    }
}