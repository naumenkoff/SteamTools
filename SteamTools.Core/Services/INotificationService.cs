using SteamTools.Core.Models;

namespace SteamTools.Core.Services;

public interface INotificationService
{
    event EventHandler<NotificationMessage> NotificationReceived;
    void ShowNotification(string message);
}