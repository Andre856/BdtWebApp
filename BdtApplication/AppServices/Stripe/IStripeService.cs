using BdtApplication.AppServices.GenericApi;
using BdtDomain.Dtos.Stripe;
using BdtDomain.Models.App;
using BdtDomain.Models.StripeModels;

namespace BdtApplication.AppServices.Stripe;

public interface IStripeService : IGenericApiService
{
    Task<StripeAPIResultModel> SendBillingInfo(StripeBillingRequest billingInfo);
    Task<string> GetPublicKey(bool isDevelopment);
    Task<ApiWrapper<SubscriptionsDto>> GetUserSubscription();
    Task<StripeAPIResultModel> CancelUserSubscription();
}
