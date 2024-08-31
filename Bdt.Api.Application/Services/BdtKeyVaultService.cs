using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Bdt.Api.Application.Services.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace Bdt.Api.Application.Services;

public class BdtKeyVaultService : IBdtKeyVaultService
{
    private readonly IMemoryCache _cache;
    private readonly SecretClient _secretClient;
    public BdtKeyVaultService(IMemoryCache cache)
    {
        _cache = cache;

        var kvUrl = Environment.GetEnvironmentVariable("AZURE_KEY_VAULT_URL")
            ?? throw new Exception("Could not find key vault url.");
        var secretClient = new SecretClient(new Uri(kvUrl), new DefaultAzureCredential());
        _secretClient = secretClient;
    }

    public string GetSecret(string secretName)
    {
        if (_cache.TryGetValue(secretName, out string secret) && secret is not null)
        {
            return secret;
        }

        secret = _secretClient.GetSecret(secretName).Value.Value;

        return secret;
    }
}
