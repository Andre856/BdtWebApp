using BdtClient.AppServices.GenericApi;
using BdtShared.Dtos.Stripe;
using BdtShared.Models.App;
using BdtShared.Models.StripeModels;

namespace BdtClient.AppServices.Stripe;

public interface IStripeService : IGenericApiService
{
    Task<StripeAPIResultModel> SendBillingInfo(StripeBillingRequest billingInfo);
    Task<string> GetPublicKey(bool isDevelopment);
    Task<ApiWrapper<SubscriptionsDto>> GetUserSubscription();
    Task<StripeAPIResultModel> CancelUserSubscription();
}
