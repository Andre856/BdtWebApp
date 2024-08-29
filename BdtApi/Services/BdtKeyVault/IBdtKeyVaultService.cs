namespace BDtApi.ApiServices.BdtKeyVault;

public interface IBdtKeyVaultService
{
    string GetSecret(string secretName);
}
