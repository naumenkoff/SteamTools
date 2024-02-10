namespace SteamTools.Common;

public interface INotificationService
{
    void Subscribe(EventHandler<NotificationMessage> eventHandler);
    void RegisterNotification(string message);
}