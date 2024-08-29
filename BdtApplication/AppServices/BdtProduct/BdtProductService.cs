using BdtApp.Static;
using BdtApplication.AppServices.GenericApi;
using BdtDomain.Dtos.BdtProduct;
using BdtDomain.Models.App;
using Newtonsoft.Json;

namespace BdtApplication.AppServices.BdtProduct;

public class BdtProductService : GenericApiService, IBdtProductService
{
    public BdtProductService(IHttpClientFactory httpClientFactory) : base(httpClientFactory) { }

    public async Task<ApiWrapper<IEnumerable<BdtProductDto>>> GetAllProducts()
    {
        try
        {
            if (StaticBdtProducts.BdtProductsContent is not null)
                return StaticBdtProducts.BdtProductsContent;

            var response = await _httpClient.GetAsync("v1/BdtProduct/GetAll");

            string contentString = await response.Content.ReadAsStringAsync();
            var content = JsonConvert.DeserializeObject<ApiWrapper<IEnumerable<BdtProductDto>>>(contentString);

            StaticBdtProducts.BdtProductsContent = content;
            return content;
        }
        catch (HttpRequestException ex)
        {
            return ApiWrapper<IEnumerable<BdtProductDto>>.Failed($"Request error. Exception: {ex.Message}");
        }
        catch (Exception ex)
        {
            return ApiWrapper<IEnumerable<BdtProductDto>>.Failed($"Api call failed. Exception: {ex.Message}");
        }
    }
}
