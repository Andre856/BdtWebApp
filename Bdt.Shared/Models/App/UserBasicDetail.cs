namespace Bdt.Shared.Models.App;

public class UserBasicDetail
{
    public string UserName { get; set; }
    public string UserId { get; set; }
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    public string Email { get; set; }
    public bool IsDarkMode { get; set; }
    public TimeSpan NotificationTime { get; set; }
    public bool IsFirstLogin { get; set; }
}
