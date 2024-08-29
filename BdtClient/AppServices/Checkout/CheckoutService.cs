using BdtClient.AppServices.GenericApi;
using BdtShared.Dtos.BdtProduct;
using BdtShared.Models.StripeModels;
using Newtonsoft.Json;
using System.Net.Http.Json;

namespace BdtClient.AppServices.Checkout;

public class CheckoutService : GenericApiService, ICheckoutService
{
    public CheckoutService(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
    { }

    public async Task<CheckoutOrderResponse> Checkout(BdtProductDto bdtProductDto)
    {
        var response = await _httpClient.PatchAsJsonAsync("v1/Checkout", bdtProductDto);

        response.EnsureSuccessStatusCode();

        var responseBody = await response.Content.ReadAsStringAsync();

        var checkoutOrderResponse = JsonConvert.DeserializeObject<CheckoutOrderResponse>(responseBody);

        return checkoutOrderResponse;
    }
}
