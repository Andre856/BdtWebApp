namespace BdtApplication.AppServices.GenericApi;

public class GenericApiService : IGenericApiService
{
    public readonly HttpClient _httpClient;

    public GenericApiService(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("BdtApi");
    }
}