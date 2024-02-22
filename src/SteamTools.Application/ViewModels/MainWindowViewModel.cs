using System;
using System.Windows.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using SteamTools.Common;
using SteamTools.Presentation.Services.Navigation;

namespace SteamTools.Presentation.ViewModels;

public class MainWindowViewModel : ObservableObject
{
    #region Constructor

    public MainWindowViewModel(INavigationService navigationService, INotificationService notificationService)
    {
        #region Private Fields

        _timer = new DispatcherTimer();
        _timer.Interval = TimeSpan.FromSeconds(1);
        _timer.Tick += DispatcherTimer_OnTick;
        _timer.Start();

        #endregion

        #region Public Properties

        Navigation = navigationService;

        #endregion

        notificationService.Subscribe(OnNotificationReceived);
    }

    #endregion

    #region Private Fields

    private readonly DispatcherTimer _timer;
    private NotificationMessage _notificationMessage;
    private bool _showNotification;

    #endregion

    #region Private Methods

    private void OnNotificationReceived(object sender, NotificationMessage newNotification)
    {
        NotificationMessage = newNotification;
        ShowNotification = true;
    }

    private void DispatcherTimer_OnTick(object sender, EventArgs e)
    {
        var lastNotificationReceivedAt = NotificationMessage?.ReceivedAt;
        var timeSinceLastNotification = DateTime.Now - lastNotificationReceivedAt;
        var fadeOutTime = TimeSpan.FromSeconds(3);
        if (timeSinceLastNotification >= fadeOutTime) ShowNotification = false;
    }

    #endregion

    #region Public Properties

    public bool IsCurrentViewIDScanner
    {
        get => Navigation.CurrentView is IDScannerViewModel;
        set
        {
            if (value is false) return;

            Navigation.Navigate<IDScannerViewModel>();
            OnPropertyChanged(nameof(IsCurrentViewLocalProfileScanner));
            OnPropertyChanged(nameof(IsCurrentViewProfileDataFetcher));
            OnPropertyChanged();
        }
    }

    public bool IsCurrentViewLocalProfileScanner
    {
        get => Navigation.CurrentView is LocalProfileScannerViewModel;
        set
        {
            if (value is false) return;

            Navigation.Navigate<LocalProfileScannerViewModel>();
            OnPropertyChanged(nameof(IsCurrentViewIDScanner));
            OnPropertyChanged(nameof(IsCurrentViewProfileDataFetcher));
            OnPropertyChanged();
        }
    }

    public bool IsCurrentViewProfileDataFetcher
    {
        get => Navigation.CurrentView is ProfileDataFetcherViewModel;
        set
        {
            if (value is false) return;

            Navigation.Navigate<ProfileDataFetcherViewModel>();
            OnPropertyChanged(nameof(IsCurrentViewIDScanner));
            OnPropertyChanged(nameof(IsCurrentViewLocalProfileScanner));
            OnPropertyChanged();
        }
    }

    public NotificationMessage NotificationMessage
    {
        get => _notificationMessage;
        private set
        {
            if (_notificationMessage == value) return;

            _notificationMessage = value;
            OnPropertyChanged();
        }
    }

    public bool ShowNotification
    {
        get => _showNotification;
        set
        {
            if (_showNotification == value) return;

            _showNotification = value;
            OnPropertyChanged();
        }
    }

    public INavigationService Navigation { get; }

    #endregion

    #region Public Commands

    #endregion
}