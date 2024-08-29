using Plugin.LocalNotification;

namespace BdtShared.Static;

public static class UserNotifications
{
    public static List<NotificationRequest> NotificationRequests { get; set; } = new List<NotificationRequest>();
    public static NotificationRequest Monday { get; set; }
    public static NotificationRequest Tuesday { get; set; }
    public static NotificationRequest Wednesday { get; set; }
    public static NotificationRequest Thursday { get; set; }
    public static NotificationRequest Friday { get; set; }
    public static NotificationRequest Saturday { get; set; }
    public static NotificationRequest Sunday { get; set; }
}
