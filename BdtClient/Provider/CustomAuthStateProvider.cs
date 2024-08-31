using BdtClient.Provider.Token;
using BdtShared.Models.App;
using Microsoft.AspNetCore.Components.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace BdtClient.Provider;

public class CustomAuthStateProvider : AuthenticationStateProvider
{
    private readonly ITokenProvider _tokenProvider;

    public CustomAuthStateProvider(ITokenProvider tokenProvider)
    {
        _tokenProvider = tokenProvider;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var accessToken = await _tokenProvider.GetAccessTokenAsync();

        if (!string.IsNullOrEmpty(accessToken) && !IsTokenExpired(accessToken))
        {
            var identity = new ClaimsIdentity(ParseClaimsFromJwt(accessToken), "jwt");

            return new AuthenticationState(new ClaimsPrincipal(identity));
        }

        // If token is expired or doesn't exist, clear it from localStorage
        await _tokenProvider.RemoveAccessTokenAsync();
        return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
    }

    private bool IsTokenExpired(string token)
    {
        var jwtHandler = new JwtSecurityTokenHandler();
        var jwtToken = jwtHandler.ReadJwtToken(token);
        return jwtToken.ValidTo < DateTime.UtcNow;
    }

    public async Task MarkUserAsAuthenticated(Tokens tokens)
    {
        var identity = new ClaimsIdentity(ParseClaimsFromJwt(tokens.AccessToken), "jwt");
        var user = new ClaimsPrincipal(identity);

        await _tokenProvider.SetAccessTokenAsync(tokens.AccessToken);
        await _tokenProvider.SetRefreshTokenAsync(tokens.RefreshToken);

        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
    }

    public async Task MarkUserAsLoggedOutAsync()
    {
        var identity = new ClaimsIdentity();

        await _tokenProvider.RemoveAccessTokenAsync();
        await _tokenProvider.RemoveRefreshTokenAsync();

        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(new ClaimsPrincipal(identity))));
    }

    private IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
    {
        var payload = jwt.Split('.')[1];
        var jsonBytes = ParseBase64WithoutPadding(payload);
        var keyValuePairs = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);

        return keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString()));
    }

    private byte[] ParseBase64WithoutPadding(string base64)
    {
        switch (base64.Length % 4)
        {
            case 2: base64 += "=="; break;
            case 3: base64 += "="; break;
        }
        return Convert.FromBase64String(base64);
    }
}