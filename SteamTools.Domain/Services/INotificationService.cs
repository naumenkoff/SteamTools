using SteamTools.Domain.Models;

namespace SteamTools.Domain.Services;

public interface INotificationService
{
    void Subscribe(EventHandler<NotificationMessage> eventHandler);
    void RegisterNotification(string message);
}