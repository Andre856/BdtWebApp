using BdtClient.AppServices.GenericApi;
using Bdt.Shared.Dtos.Stripe;
using Bdt.Shared.Models.App;
using Bdt.Shared.Models.StripeModels;

namespace BdtClient.AppServices.Stripe;

public interface IStripeService : IGenericApiService
{
    Task<StripeAPIResultModel> SendBillingInfo(StripeBillingRequest billingInfo);
    Task<string> GetPublicKey(bool isDevelopment);
    Task<ApiWrapper<SubscriptionsDto>> GetUserSubscription();
    Task<StripeAPIResultModel> CancelUserSubscription();
}
