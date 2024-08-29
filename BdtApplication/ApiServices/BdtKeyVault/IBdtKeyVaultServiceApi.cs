namespace BdtApplication.ApiServices.BdtKeyVault;

public interface IBdtKeyVaultServiceApi
{
    string GetSecret(string secretName);
}
