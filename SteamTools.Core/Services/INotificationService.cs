using SteamTools.Core.Models;

namespace SteamTools.Core.Services;

public interface INotificationService
{
    void Subscribe(EventHandler<NotificationMessage> eventHandler);
    void RegisterNotification(string message);
}