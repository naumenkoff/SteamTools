using System;
using SteamTools.Core.Enums;
using SteamTools.Core.Models;
using SteamTools.Core.Services;

namespace SteamTools.UI.Services.Notifications;

public class PopupNotificationService : INotificationService
{
    public event EventHandler<NotificationMessage> NotificationReceived;

    public void ShowNotification(string message, NotificationLevel notificationLevel)
    {
        NotificationReceived?.Invoke(this, new NotificationMessage(message, notificationLevel));
    }
}