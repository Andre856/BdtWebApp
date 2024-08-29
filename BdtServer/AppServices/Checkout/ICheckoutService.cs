using BdtServer.AppServices.GenericApi;
using BdtShared.Dtos.BdtProduct;
using BdtShared.Models.StripeModels;

namespace BdtServer.AppServices.Checkout;

public interface ICheckoutService : IGenericApiService
{
    Task<CheckoutOrderResponse> Checkout(BdtProductDto bdtProductDto);
}
