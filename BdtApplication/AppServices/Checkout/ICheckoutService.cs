using BdtApplication.AppServices.GenericApi;
using BdtDomain.Dtos.BdtProduct;
using BdtDomain.Models.StripeModels;

namespace BdtApplication.AppServices.Checkout;

public interface ICheckoutService : IGenericApiService
{
    Task<CheckoutOrderResponse> Checkout(BdtProductDto bdtProductDto);
}
