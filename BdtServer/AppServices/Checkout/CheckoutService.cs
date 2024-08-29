using BdtServer.AppServices.GenericApi;
using BdtShared.Dtos.BdtProduct;
using BdtShared.Models.StripeModels;
using Newtonsoft.Json;

namespace BdtServer.AppServices.Checkout;

public class CheckoutService : GenericApiService, ICheckoutService
{
    public CheckoutService(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
    { }

    public async Task<CheckoutOrderResponse> Checkout(BdtProductDto bdtProductDto)
    {
        var response = await _httpClient.PostAsJsonAsync("v1/Checkout", bdtProductDto);

        response.EnsureSuccessStatusCode();

        var responseBody = await response.Content.ReadAsStringAsync();

        var checkoutOrderResponse = JsonConvert.DeserializeObject<CheckoutOrderResponse>(responseBody);

        return checkoutOrderResponse;
    }
}
