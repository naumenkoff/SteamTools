using SteamTools.Core.Enums;

namespace SteamTools.Core.Models;

public class NotificationMessage
{
    public NotificationMessage(string text, NotificationLevel notificationLevel)
    {
        Text = text;
        NotificationLevel = notificationLevel;
        RecivedAt = DateTime.Now;
    }

    public DateTime RecivedAt { get; }
    public NotificationLevel NotificationLevel { get; }
    public string Text { get; }
}