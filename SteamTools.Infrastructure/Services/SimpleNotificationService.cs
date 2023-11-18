using SteamTools.Domain.Models;
using SteamTools.Domain.Services;

namespace SteamTools.Infrastructure.Services;

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
}