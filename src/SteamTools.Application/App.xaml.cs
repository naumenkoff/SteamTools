using System;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SProject.CQRS;
using SProject.DependencyInjection;
using SProject.Steam;
using SteamTools.Common;
using SteamTools.Presentation.Services.Navigation;
using SteamTools.Presentation.ViewModels;
using SteamTools.Presentation.Views;
using SteamTools.ProfileFetcher;
using SteamTools.ProfileScanner;
using SteamTools.SignatureSearcher;

namespace SteamTools.Presentation;

/// <summary>
///     Composition Root
/// </summary>
public partial class App
{
    private readonly IHost _host = CreateApplicationHost();

    protected override async void OnStartup(StartupEventArgs e)
    {
        await _host.StartAsync();

        var navigator = _host.Services.GetRequiredService<INavigationService>();
        navigator.Navigate<ProfileDataFetcherViewModel>();

        var main = _host.Services.GetRequiredService<MainWindow>();
        main.Show();

        base.OnStartup(e);
    }

    private static IHost CreateApplicationHost()
    {
        var builder = Host.CreateApplicationBuilder();

        builder.Services.AddSingleton(typeof(IServiceScopeFactory<>), typeof(ServiceScopeFactory<>));

        builder.Services.AddSingleton<Func<Type, ObservableObject>>(serviceProvider =>
            viewModelType => (ObservableObject)serviceProvider.GetRequiredService(viewModelType));

        builder.Services.AddSingleton(provider => new MainWindow
        {
            DataContext = provider.GetRequiredService<MainWindowViewModel>()
        });

        builder.Services.RegisterRequestHandlers();
        builder.Services.AddSingleton<MainWindowViewModel>();
        builder.Services.AddSingleton<IDScannerViewModel>();
        builder.Services.AddSingleton<LocalProfileScannerViewModel>();
        builder.Services.AddSingleton<ProfileDataFetcherViewModel>();
        builder.Services.AddSingleton<INavigationService, NavigationService>();

        builder.Services.AddSignatureSearcher();
        builder.Services.AddSingleton<ScanningOptions>();
        builder.Services.AddProfileFetcher(builder.Configuration);
        builder.Services.AddProfileScanner();

        builder.Services.AddSteamClient();
        builder.Services.AddSingleton<SteamClient>();

        builder.Services.AddSingleton<INotificationService, SimpleNotificationService>();

        return builder.Build();
    }
}