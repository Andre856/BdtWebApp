using BdtClient.AppServices.GenericApi;
using BdtShared.Dtos.Stripe;
using BdtShared.Enums;
using BdtShared.Models.App;
using BdtShared.Models.StripeModels;
using Newtonsoft.Json;
using System.Text;

namespace BdtClient.AppServices.Stripe;

public class StripeService : GenericApiService, IStripeService
{
    public StripeService(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
    { }

    public async Task<StripeAPIResultModel> CancelUserSubscription()
    {
        try
        {
            var response = await _httpClient.GetAsync("v1/Stripe/CancelSubscription");

            string contentString = await response.Content.ReadAsStringAsync();
            var content = JsonConvert.DeserializeObject<StripeAPIResultModel>(contentString);

            return content;
        }
        catch (HttpRequestException ex)
        {
            return new StripeAPIResultModel(false, $"Request error. Exception: {ex.Message}");
        }
        catch (Exception ex)
        {
            return new StripeAPIResultModel(false, $"Api call failed. Exception: {ex.Message}");
        }
    }
    public async Task<ApiWrapper<SubscriptionsDto>> GetUserSubscription()
    {
        try
        {
            var response = await _httpClient.GetAsync("v1/Stripe/GetUserSubscription");

            string contentString = await response.Content.ReadAsStringAsync();
            var content = JsonConvert.DeserializeObject<ApiWrapper<SubscriptionsDto>>(contentString);

            return content;
        }
        catch (HttpRequestException ex)
        {
            return ApiWrapper<SubscriptionsDto>.Failed($"Request error. Exception: {ex.Message}");
        }
        catch (Exception ex)
        {
            return ApiWrapper<SubscriptionsDto>.Failed($"Api call failed. Exception: {ex.Message}");
        }
    }

    public async Task<StripeAPIResultModel> SendBillingInfo(StripeBillingRequest billingInfo)
    {
        var serialisedString = JsonConvert.SerializeObject(billingInfo);

        try
        {
            string contentString = string.Empty;
            if (Globals.Environment == EnvironmentEnums.Devevelopment)
            {
                var response = await _httpClient.PostAsync("v1/Stripe/ActivateTestSubscription", new StringContent(serialisedString, Encoding.UTF8, "application/json"));
                contentString = await response.Content.ReadAsStringAsync();
            }
            else
            {
                var response = await _httpClient.PostAsync("v1/Stripe/ActivateProdSubscription", new StringContent(serialisedString, Encoding.UTF8, "application/json"));
                contentString = await response.Content.ReadAsStringAsync();
            }

            var content = JsonConvert.DeserializeObject<StripeAPIResultModel>(contentString);

            return content;
        }
        catch (Exception ex)
        {
            var failed = new StripeAPIResultModel { Success = false };

            if (ex.Message.Contains("One or more errors"))
            {
                failed.Message = ex.InnerException.Message;
            }
            else if (ex.Message.Equals("The request message was already sent. Cannot send the same request message multiple times."))
            {
                failed.Message = "Error contacting Server Please try again later";
            }
            else
            {
                failed.Message = ex.Message;
            }

            return failed;
        }
    }

    public async Task<string> GetPublicKey(bool isDevelopment)
    {
        try
        {
            if (isDevelopment)
            {
                var response = await _httpClient.GetAsync("v1/Stripe/GetTestPublicKey");
                if (response.IsSuccessStatusCode)
                {
                    string contentString = await response.Content.ReadAsStringAsync();

                    return contentString;
                }
                else
                {
                    return "Error: Could not find key.";
                }
            }
            else
            {
                var response = await _httpClient.GetAsync("v1/Stripe/GetProdPublicKey");
                if (response.IsSuccessStatusCode)
                {
                    string contentString = await response.Content.ReadAsStringAsync();

                    return contentString;
                }
                else
                {
                    return "Error: Could not find key.";
                }
            }
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
}
