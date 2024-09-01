using Bdt.Shared.Models.App;

namespace Bdt.Client.AppServices.Session;

public interface ISessionService
{
    Task SaveAccessTokenAsync(string accessToken);
    Task SaveRefreshTokenAsync(string refreshToken);
    Task<string> GetAccessTokenAsync();
    Task<string> GetRefreshTokenAsync();
    Task ClearSessionAsync();
    Task<string> GetUserEmailAsync();
    Task SaveUserEmailAsync(string email);
    Task SaveUserDetailsAsync(UserBasicDetail userBasicDetail);
    Task<UserBasicDetail> GetUserDetailsAsync();
}
