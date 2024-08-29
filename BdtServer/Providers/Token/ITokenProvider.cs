namespace BdtServer.Providers.Token;

public interface ITokenProvider
{
    string GetAccessToken();
    void SetAccessToken(string token);
}
