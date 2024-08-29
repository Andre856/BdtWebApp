namespace BdtClient.AppServices.Claims;

public interface IClaimService
{
    Task<string> GetUserId();
    Task<string> GetUsername();
    Task<bool> IsDarkTheme();
    Task<TimeSpan> GetNotificationTime();
    Task<bool> IsFirstLogin();
    Task<string> GetEmailAsync();
}
