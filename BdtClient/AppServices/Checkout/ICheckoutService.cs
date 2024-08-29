using BdtClient.AppServices.GenericApi;
using BdtShared.Dtos.BdtProduct;
using BdtShared.Models.StripeModels;

namespace BdtClient.AppServices.Checkout;

public interface ICheckoutService : IGenericApiService
{
    Task<CheckoutOrderResponse> Checkout(BdtProductDto bdtProductDto);
}
