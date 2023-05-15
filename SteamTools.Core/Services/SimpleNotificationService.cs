using SteamTools.Core.Models;

namespace SteamTools.Core.Services;

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

    private event EventHandler<NotificationMessage> NotificationReceived;

    /*
     
     private readonly List<EventHandler<NotificationMessage>> _subscribers;
     
     public SimpleNotificationService()
     {
        _subscribers = new List<EventHandler<NotificationMessage>>();
     }
     
     public event EventHandler<NotificationMessage> NotificationReceived;
     
     public void Subscribe(EventHandler<NotificationMessage> eventHandler)
     {
        _subscribers.Add(eventHandler);
     }
     
     public void Unsubscribe(EventHandler<NotificationMessage> eventHandler)
     {
        _subscribers.Remove(eventHandler);
     }
     
     public void RegisterNotification(string message)
     {
        var notificationMessage = new NotificationMessage(message, DateTime.Now);
        foreach (var subscriber in _subscribers)
        {
            subscriber?.Invoke(this, notificationMessage);
        }
     }
     
     */
}