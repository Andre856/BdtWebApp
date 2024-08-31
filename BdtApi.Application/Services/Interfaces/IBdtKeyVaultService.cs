namespace BdtApi.Application.Services.Interfaces;

public interface IBdtKeyVaultService
{
    string GetSecret(string secretName);
}
