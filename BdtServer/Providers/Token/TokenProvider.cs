using BdtServer.AppServices.Session;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Http;

namespace BdtServer.Providers.Token;
public class TokenProvider : ITokenProvider
{
    private string _accessToken;

    public TokenProvider()
    {

    }
    public string GetAccessToken() => _accessToken;

    public void SetAccessToken(string token)
    {
        _accessToken = token;
    }
}
