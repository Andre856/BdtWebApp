namespace BdtApi.Application.Services.BdtKeyVault;

public interface IBdtKeyVaultService
{
    string GetSecret(string secretName);
}
