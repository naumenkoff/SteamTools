using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SteamTools.Core.Enums;
using SteamTools.Core.Models;
using SteamTools.Core.Services;
using SteamTools.UI.Services.Navigation;

namespace SteamTools.UI.ViewModels;

public class MainWindowViewModel : ObservableObject
{
    // TODO Убирать TextBlock, отображающий уведомление, если прошло более 5 секунд с момента его получения
    // TODO Помечать цветом полученное уведомление
    // TODO разобраться, где нужны уведомления, а где нет.

    private NotificationMessage _notificationMessage;

    public MainWindowViewModel(INavigationService navigationService, INotificationService notificationService)
    {
        Navigation = navigationService;

        ProfileDataFetcherCommand = new RelayCommand(() => Navigation.Navigate<ProfileDataFetcherViewModel>());
        LocalProfileScannerCommand = new RelayCommand(() => Navigation.Navigate<LocalProfileScannerViewModel>());
        IDScannerCommand = new RelayCommand(() => Navigation.Navigate<IDScannerViewModel>());

        MoveApplicationCommand = new RelayCommand(MoveApplicationWindow);
        ShutdownApplicationCommand = new RelayCommand(ShutdownApplication);
        MinimizeApplicationCommand = new RelayCommand(MinimizeApplication);

        notificationService.NotificationReceived += OnNotificationReceived;
    }

    public NotificationMessage NotificationMessage
    {
        get => _notificationMessage;
        set
        {
            _notificationMessage = value;
            OnPropertyChanged();
        }
    }

    public INavigationService Navigation { get; }

    public RelayCommand IDScannerCommand { get; }
    public RelayCommand LocalProfileScannerCommand { get; }
    public RelayCommand ProfileDataFetcherCommand { get; }

    public RelayCommand ShutdownApplicationCommand { get; }
    public RelayCommand MoveApplicationCommand { get; }
    public RelayCommand MinimizeApplicationCommand { get; }

    private void OnNotificationReceived(object sender, NotificationMessage newNotification)
    {
        var isCurrentNotificationWarning = NotificationMessage?.NotificationLevel == NotificationLevel.Warning;
        var isNewNotificationWarning = newNotification.NotificationLevel == NotificationLevel.Warning;

        switch (isCurrentNotificationWarning)
        {
            case true when isNewNotificationWarning:
            default:
                NotificationMessage = newNotification;
                break;
            case true:
            {
                var timeSinceLastNotification = newNotification.RecivedAt - NotificationMessage?.RecivedAt;
                if (timeSinceLastNotification?.TotalSeconds > 1) NotificationMessage = newNotification;
                break;
            }
        }
    }

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