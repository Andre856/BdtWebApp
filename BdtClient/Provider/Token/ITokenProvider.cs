namespace BdtClient.Provider.Token;

public interface ITokenProvider
{
    Task<string> GetAccessTokenAsync();
    Task SetAccessTokenAsync(string token);
    Task RemoveAccessTokenAsync();
    Task<string> GetRefreshTokenAsync();
    Task SetRefreshTokenAsync(string token);
    Task RemoveRefreshTokenAsync();
}
