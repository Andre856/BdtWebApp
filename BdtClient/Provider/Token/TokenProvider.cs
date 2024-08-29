using Microsoft.JSInterop;

namespace BdtClient.Provider.Token;

public class TokenProvider : ITokenProvider
{
    private readonly IJSRuntime _jsRuntime;
    private const string ACCESS_TOKEN_KEY = "authToken";
    private const string REFRESH_TOKEN_KEY = "refreshToken";

    public TokenProvider(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public async Task<string> GetAccessTokenAsync()
    {
        return await _jsRuntime.InvokeAsync<string>("localStorage.getItem", ACCESS_TOKEN_KEY);
    }

    public async Task SetAccessTokenAsync(string token)
    {
        await _jsRuntime.InvokeVoidAsync("localStorage.setItem", ACCESS_TOKEN_KEY, token);
    }

    public async Task RemoveAccessTokenAsync()
    {
        await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", ACCESS_TOKEN_KEY);
    }

    public async Task<string> GetRefreshTokenAsync()
    {
        return await _jsRuntime.InvokeAsync<string>("localStorage.getItem", REFRESH_TOKEN_KEY);
    }

    public async Task SetRefreshTokenAsync(string token)
    {
        await _jsRuntime.InvokeVoidAsync("localStorage.setItem", REFRESH_TOKEN_KEY, token);
    }

    public async Task RemoveRefreshTokenAsync()
    {
        await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", REFRESH_TOKEN_KEY);
    }
}
