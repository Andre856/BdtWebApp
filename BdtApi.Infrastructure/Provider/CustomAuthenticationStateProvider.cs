using BdtShared.Models.App;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace BdtApp.Providers;

public class CustomAuthenticationStateProvider : AuthenticationStateProvider
{
    private ClaimsPrincipal _anonymous = new ClaimsPrincipal(new ClaimsIdentity());

    public async Task MarkUserAsAuthenticated(UserBasicDetail userBasicDetail)
    {
        var identity = new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.Name, userBasicDetail.UserName),
            new Claim(ClaimTypes.Email, userBasicDetail.Email),
            new Claim(ClaimTypes.NameIdentifier, userBasicDetail.UserId),
            new Claim("IsDarkMode", userBasicDetail.IsDarkMode.ToString()),
            new Claim("NotificationTime", userBasicDetail.NotificationTime.ToString(@"hh\:mm\:ss")),
            new Claim("IsFirstLogin", userBasicDetail.IsFirstLogin.ToString()),
            new Claim("AccessToken", userBasicDetail.AccessToken),
            new Claim("RefreshToken", userBasicDetail.RefreshToken)
        }, "apiauth_type");

        var user = new ClaimsPrincipal(identity);

        // Update _anonymous to reflect the authenticated user
        _anonymous = user;

        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
    }

    public void MarkUserAsLoggedOut()
    {
        _anonymous = new ClaimsPrincipal(new ClaimsIdentity()); // Update _anonymous to be anonymous again
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_anonymous)));
    }

    public override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        // Return the current user or an anonymous user
        return Task.FromResult(new AuthenticationState(_anonymous));
    }

    public string GetAccessToken()
    {
        var accessTokenClaim = _anonymous.Claims.FirstOrDefault(c => c.Type == "AccessToken");
        return accessTokenClaim?.Value;
    }
}
