namespace SteamTools.Common;

public class SimpleNotificationService : INotificationService
{
    public void Subscribe(EventHandler<NotificationMessage> eventHandler)
    {
        NotificationReceived += eventHandler;
    }

    public void RegisterNotification(string message)
    {
        NotificationReceived?.Invoke(this, new NotificationMessage(message, DateTime.Now));
    }

    private event EventHandler<NotificationMessage>? NotificationReceived;
}