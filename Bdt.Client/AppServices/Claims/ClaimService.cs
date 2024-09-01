using Bdt.Client.Provider;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Bdt.Client.AppServices.Claims;

public class ClaimService : IClaimService
{
    private readonly CustomAuthStateProvider _authStateProvider;

    public ClaimService(CustomAuthStateProvider authStateProvider)
    {
        _authStateProvider = authStateProvider;
    }

    private async Task<ClaimsPrincipal> GetUser()
    {
        var authState = await _authStateProvider.GetAuthenticationStateAsync();
        return authState.User;
    }

    public async Task<string> GetUserId()
    {
        var user = await GetUser();
        return user.FindFirst(JwtRegisteredClaimNames.NameId)?.Value;
    }

    public async Task<string> GetUsername()
    {
        var user = await GetUser();
        return user.FindFirst(JwtRegisteredClaimNames.UniqueName)?.Value;
    }

    public async Task<bool> IsDarkTheme()
    {
        var user = await GetUser();
        var claimValue = user.FindFirst("IsDarkTheme")?.Value;
        return !string.IsNullOrEmpty(claimValue) && claimValue.ToLower() == "true";
    }

    public async Task<TimeSpan> GetNotificationTime()
    {
        var user = await GetUser();
        var claimValue = user.FindFirst("NotificationTime")?.Value;
        return !string.IsNullOrEmpty(claimValue) ? TimeSpan.ParseExact(claimValue, "hh\\:mm\\:ss", CultureInfo.InvariantCulture) : TimeSpan.Zero;
    }

    public async Task<bool> IsFirstLogin()
    {
        var user = await GetUser();
        var claimValue = user.FindFirst("IsFirstLogin")?.Value;
        return !string.IsNullOrEmpty(claimValue) && claimValue.ToLower() == "true";
    }

    public async Task<string> GetEmailAsync()
    {
        var user = await GetUser();
        return user.FindFirst(JwtRegisteredClaimNames.Email)?.Value;
    }
}
