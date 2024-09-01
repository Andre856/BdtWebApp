using Bdt.Shared.Dtos.BdtProduct;
using Bdt.Shared.Models.StripeModels;
using Bdt.Client.AppServices.GenericApi;

namespace Bdt.Client.AppServices.Checkout;

public interface ICheckoutService : IGenericApiService
{
    Task<CheckoutOrderResponse> Checkout(BdtProductDto bdtProductDto);
}
