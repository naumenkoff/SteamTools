using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SteamTools.UI.Services.Navigation;

namespace SteamTools.UI.ViewModels;

public class MainWindowViewModel : ObservableObject
{
    public MainWindowViewModel(INavigationService navigationService)
    {
        Navigation = navigationService;

        ProfileDataFetcherCommand = new RelayCommand(() => Navigation.Navigate<ProfileDataFetcherViewModel>());
        LocalProfileScannerCommand = new RelayCommand(() => Navigation.Navigate<LocalProfileScannerViewModel>());
        IDScannerCommand = new RelayCommand(() => Navigation.Navigate<IDScannerViewModel>());

        MoveApplicationCommand = new RelayCommand(MoveApplicationWindow);
        ShutdownApplicationCommand = new RelayCommand(ShutdownApplication);
        MinimizeApplicationCommand = new RelayCommand(MinimizeApplication);
    }

    public INavigationService Navigation { get; }

    public RelayCommand IDScannerCommand { get; }
    public RelayCommand LocalProfileScannerCommand { get; }
    public RelayCommand ProfileDataFetcherCommand { get; }

    public RelayCommand ShutdownApplicationCommand { get; }
    public RelayCommand MoveApplicationCommand { get; }
    public RelayCommand MinimizeApplicationCommand { get; }

    private static void MinimizeApplication()
    {
        Application.Current.MainWindow!.WindowState = WindowState.Minimized;
    }

    private static void ShutdownApplication()
    {
        Application.Current.Shutdown();
    }

    private static void MoveApplicationWindow()
    {
        Application.Current.MainWindow!.DragMove();
    }
}