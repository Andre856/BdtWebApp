using BdtClient.AppServices.GenericApi;
using Bdt.Shared.Dtos.BdtProduct;
using Bdt.Shared.Models.StripeModels;

namespace BdtClient.AppServices.Checkout;

public interface ICheckoutService : IGenericApiService
{
    Task<CheckoutOrderResponse> Checkout(BdtProductDto bdtProductDto);
}
