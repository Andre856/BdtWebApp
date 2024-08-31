namespace BdtShared.Dtos.Users;

public class UpdateUserNotificationTimeDto
{
    public string Email { get; set; }
    public TimeSpan NotificationTime { get; set; }
}
