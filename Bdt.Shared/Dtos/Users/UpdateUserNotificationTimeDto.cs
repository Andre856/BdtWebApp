namespace Bdt.Shared.Dtos.Users;

public class UpdateUserNotificationTimeDto
{
    public string Email { get; set; }
    public TimeSpan NotificationTime { get; set; }
}
