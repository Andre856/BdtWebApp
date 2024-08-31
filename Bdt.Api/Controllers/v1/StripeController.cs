using Bdt.Api.Infrastructure.Managers;
using Bdt.Shared.Dtos.Stripe;
using Bdt.Shared.Models.App;
using Bdt.Shared.Models.StripeModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Bdt.Api.Controllers.v1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Authorize]
public class StripeController : ControllerBase
{
    private readonly ILogger _logger;
    private readonly IConfiguration _configuration;
    private readonly StripeManager _stripeMan;
    public StripeController(ILogger<StripeController> logger, IConfiguration configuration, StripeManager stripeManager)
    {
        _logger = logger;
        _configuration = configuration;
        _stripeMan = stripeManager;
    }

    [HttpGet("GetUserSubscription")]
    public async Task<ApiWrapper<SubscriptionsDto>> GetUserSubscription()
    {
        string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? throw new Exception("User not found.");

        var subscriptionDto = await _stripeMan.GetSubscriptionByUserId(userId);

        return subscriptionDto;
    }

    [HttpGet("CancelSubscription")]
    public async Task<StripeAPIResultModel> CancelSubscription()
    {
        string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId is null)
        {
            return new StripeAPIResultModel { Success = false, Message = "User not found." };
        }

        var result = await _stripeMan.CancelSubscription(userId);

        return result;
    }

    [HttpPost("ActivateTestSubscription")]
    public async Task<StripeAPIResultModel> ActivateTestSubscription(StripeBillingRequest billingRequest)
    {
        string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId is null)
        {
            return new StripeAPIResultModel { Success = false, Message = "User not found." };
        }

        var result = _stripeMan.VerifySubRequest(billingRequest);
        if (result.Success == false)
            return result;

        try
        {
            var x = await _stripeMan.ActivateSubscriptionAsync(billingRequest, userId, isTest: true);
            return x;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error Creating Stripe Sub");
            result.Success = false;
            result.Message = "Error activating your subscription, please try again later. If the problem persists, please contact our support team.";
        }

        return result;
    }

    [HttpPost("ActivateProdSubscription")]
    public async Task<StripeAPIResultModel> ActivateProdSubscription(StripeBillingRequest billingRequest)
    {
        string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId is null)
        {
            return new StripeAPIResultModel { Success = false, Message = "User not found." };
        }

        var result = _stripeMan.VerifySubRequest(billingRequest);
        if (result.Success == false)
            return result;

        try
        {
            return await _stripeMan.ActivateSubscriptionAsync(billingRequest, userId, isTest: false);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error Creating Stripe Sub");
            result.Success = false;
            result.Message = "Error activating your subscription, please try again later. If the problem persists, please contact our support team.";
        }

        return result;
    }

    [HttpGet("GetTestPublicKey")]
    public async Task<ActionResult<string>> GetTestPublicKey()
    {
        var stripeTestPublicKey = _configuration.GetSection("StripeTestPubKey").Value;

        if (string.IsNullOrEmpty(stripeTestPublicKey))
            return NotFound("Key was not found.");

        return Ok(stripeTestPublicKey);
    }

    [HttpGet("GetProdPublicKey")]
    public async Task<ActionResult<string>> GetProdPublicKey()
    {
        var stripeProdPublicKey = _configuration.GetSection("StripeProdPubKey").Value;

        if (string.IsNullOrEmpty(stripeProdPublicKey))
            return NotFound("Key was not found.");

        return Ok(stripeProdPublicKey);
    }
}
