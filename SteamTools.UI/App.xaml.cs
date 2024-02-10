using System;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SProject.Steam;
using SteamTools.Common;
using SteamTools.ProfileFetcher;
using SteamTools.ProfileFetcher.DependencyInjection;
using SteamTools.ProfileScanner.DependencyInjection;
using SteamTools.SignatureSearcher.DependencyInjection;
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
        _configuration = BuildConfiguration();
        _serviceProvider = ConfigureServices();
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        var navigator = _serviceProvider.GetRequiredService<INavigationService>();
        navigator.Navigate<ProfileDataFetcherViewModel>();

        var main = _serviceProvider.GetRequiredService<MainWindow>();
        main.Show();

        base.OnStartup(e);
    }

    private static IConfiguration BuildConfiguration()
    {
        var configuration = new ConfigurationBuilder();
        configuration.AddJsonFile("appsettings.json");
        return configuration.Build();
    }

    private ServiceProvider ConfigureServices()
    {
        var services = new ServiceCollection();

        services.AddSingleton<Func<IProfileFetcherService>>(serviceProvider => serviceProvider.GetRequiredService<IProfileFetcherService>);

        #region UI

        services.AddSingleton<Func<Type, ObservableObject>>(serviceProvider =>
            viewModelType => (ObservableObject)serviceProvider.GetRequiredService(viewModelType));
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

        services.AddSignatureSearcher();
        services.AddSingleton<ScanningOptions>();
        services.AddProfileFetcher();
        services.AddProfileScanner();

        #region Core

        services.AddSteamClient();
        services.AddSingleton<SteamClient>();

        services.AddSingleton<INotificationService, SimpleNotificationService>();

        #endregion

        return services.BuildServiceProvider();
    }
}