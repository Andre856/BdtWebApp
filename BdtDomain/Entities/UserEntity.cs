using Microsoft.AspNetCore.Identity;

namespace BdtShared.Entities;

public class UserEntity : IdentityUser, IEntity<string>
{
    public string? RefreshToken { get; set; }
    public bool IsDarkTheme { get; set; }
    public TimeSpan NotificationTime { get; set; } = new TimeSpan(6, 0, 0);
    public bool IsFirstLogin { get; set; } = true;
}
