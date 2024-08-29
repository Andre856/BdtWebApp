using AutoMapper;
using BdtShared.Dtos.BdtProduct;
using BdtShared.Entities;
using BdtShared.Models.StripeModels;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Stripe;
using Stripe.Checkout;

namespace BdtApi.Controllers.v1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiExplorerSettings(IgnoreApi = true)]
public class CheckoutController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly IMapper _mapper;
    private readonly ILogger _logger;
    private static string s_wasmClientURL = string.Empty;

    public CheckoutController(IConfiguration configuration, IMapper mapper, ILogger<CheckoutController> logger)
    {
        _configuration = configuration;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ActionResult> CheckoutOrder([FromBody] BdtProductDto bdtProductDto, [FromServices] IServiceProvider sp)
    {
        // Build the URL to which the customer will be redirected after paying.
        var server = sp.GetRequiredService<IServer>();

        var serverAddressesFeature = server.Features.Get<IServerAddressesFeature>();

        string? thisApiUrl = null;

        if (serverAddressesFeature is not null)
        {
            thisApiUrl = serverAddressesFeature.Addresses.FirstOrDefault();
        }

        if (thisApiUrl is not null)
        {
            var referer = Request.Headers.Referer;
            if (!string.IsNullOrEmpty(referer))
            {
                s_wasmClientURL = referer[0];
            }
            else
            {
                s_wasmClientURL = thisApiUrl;
            }

            var bdtProduct = _mapper.Map<BdtProductEntity>(bdtProductDto);
            var sessionId = await CheckOut(bdtProduct, thisApiUrl);
            var pubKey = _configuration["StripeTestPubKey"];

            var checkoutOrderResponse = new CheckoutOrderResponse()
            {
                SessionId = sessionId,
                PubKey = pubKey
            };

            return Ok(checkoutOrderResponse);
        }
        else
        {
            return StatusCode(500);
        }
    }

    [NonAction]
    public async Task<string> CheckOut(BdtProductEntity bdtProduct, string thisApiUrl)
    {
        // Create a payment flow from the items in the cart.
        // Gets sent to Stripe API.
        var options = new SessionCreateOptions
        {
            // Stripe calls the URLs below when certain checkout events happen such as success and failure.
            SuccessUrl = $"{thisApiUrl}/checkout/success?sessionId=" + "{CHECKOUT_SESSION_ID}", // Customer paid.
            CancelUrl = $"{s_wasmClientURL}/failed",  // Checkout cancelled.
            PaymentMethodTypes = new List<string> // Only card available in test mode?
            {
                "card"
            },
            LineItems = new List<SessionLineItemOptions>
            {
                new()
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = bdtProduct.Price, // Price is in USD cents.
                        Recurring =  new SessionLineItemPriceDataRecurringOptions
                        {
                            Interval = bdtProduct.BillingInterval,
                            IntervalCount = bdtProduct.BillingInterval == PlanIntervals.Month ? 12 : 1
                        },
                        Currency = "USD",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = bdtProduct.Title,
                            Description = bdtProduct.Description
                        },
                    },
                    Quantity = 1,
                },
            },
            Mode = "subscription" // One-time payment. Stripe supports recurring 'subscription' payments.
        };

        var service = new SessionService();

        try
        {
            var session = await service.CreateAsync(options);
            return session.Id;
        }
        catch (Exception ex)
        {
            _logger.LogError(message: "There was an error", ex);
            return "";
        }
    }

    [HttpGet("success")]
    // Automatic query parameter handling from ASP.NET.
    // Example URL: https://localhost:7051/checkout/success?sessionId=si_123123123123
    public ActionResult CheckoutSuccess(string sessionId)
    {
        var sessionService = new SessionService();
        var session = sessionService.Get(sessionId);

        // Here you can save order and customer details to your database.
        var total = session.AmountTotal.Value;
        var customerEmail = session.CustomerDetails.Email;

        return Redirect(s_wasmClientURL + "success");
    }
}
