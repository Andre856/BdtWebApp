using AutoMapper;
using BdtDomain.Dtos.Stripe;
using BdtApi.Context;
using BdtDomain.Entities;
using BdtDomain.Models.App;
using BdtDomain.Models.StripeModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Stripe;

namespace BdtApi.Managers
{
    public class StripeManager
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly BdtDbContext _context;
        public StripeManager(ILogger<StripeManager> logger, IConfiguration configuration, IMapper mapper, BdtDbContext context)
        {
            _logger = logger;
            _mapper = mapper;
            _context = context;
            _configuration = configuration;
        }

        public async Task<ApiWrapper<SubscriptionsDto>> GetSubscriptionByUserId(string userId)
        {
            try
            {
                var subscriptionEntity = await _context.Subscriptions.FirstOrDefaultAsync(x => x.UserId == userId);
                var subscriptionDto = _mapper.Map<SubscriptionsDto>(subscriptionEntity);

                var result = ApiWrapper<SubscriptionsDto>.Success(subscriptionDto);

                return result;
            }
            catch (Exception ex)
            {
                return ApiWrapper<SubscriptionsDto>.Failed(ex.Message);
            }
        }

        public StripeAPIResultModel VerifySubRequest(StripeBillingRequest subReq, bool checkDb = false)
        {
            if (subReq == null)
            {
                return new StripeAPIResultModel(false, "Subscription Request cannot be empty.");
            }
            else if (string.IsNullOrWhiteSpace(subReq.BillingName))
            {
                return new StripeAPIResultModel(false, "Billing Name is required.");
            }
            else if (string.IsNullOrWhiteSpace(subReq.BillingEmail))
            {
                return new StripeAPIResultModel(false, "Billing Email is required.");
            }
            else if (string.IsNullOrWhiteSpace(subReq.PaymentMethod))
            {
                return new StripeAPIResultModel(false, "Billing payment ID is required.");
            }
            else if (string.IsNullOrWhiteSpace(subReq.Price))
            {
                return new StripeAPIResultModel(false, "Billing Plan ID is required.");
            }
            else if (checkDb)
            {
                var dbPlan = _context.BdtProduct.FirstOrDefault(x => x.StripeTestApiId == subReq.Price);
                if (dbPlan == null)
                {
                    return new StripeAPIResultModel(false, "Billing Plan ID was not found.");
                }
            }

            return new StripeAPIResultModel(true);
        }

        public async Task<StripeAPIResultModel> ActivateSubscriptionAsync(StripeBillingRequest subReq, string userId, bool isTest)
        {
            StripeAPIResultModel result = VerifySubRequest(subReq, false);
            if (!result.Success)
            {
                return result;
            }

            BdtProductEntity? dbPlan = null;
            if (isTest)
            {
                StripeConfiguration.ApiKey = _configuration.GetSection("StripeTestSecretKey").Value;
                dbPlan = _context.BdtProduct.FirstOrDefault(x => x.StripeTestApiId == subReq.Price);
            }
            else
            {
                StripeConfiguration.ApiKey = _configuration.GetSection("StripeProdSecretKey").Value;
                dbPlan = _context.BdtProduct.FirstOrDefault(x => x.StripeProdApiId == subReq.Price);
            }

            if (dbPlan is null)
            {
                result.Success = false;
                result.Message = "Billing Plan ID was not found";
            }

            if (!result.Success)
            {
                return result;
            }

            string customerID = await CheckIfCustomerExistsOrCreateOneAsync(subReq, userId);
            await UpdateCustomersDefaultPaymentIDAsync(customerID, subReq.PaymentMethod);

            try
            {
                result = await CreateSubscriptionAsync(userId, customerID, subReq);
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = ex.Message;
            }

            return result;
        }

        public async Task<StripeAPIResultModel> CancelSubscription(string userId)
        {
            var subscription = _context.Subscriptions.FirstOrDefault(x => x.UserId == userId);

            if (subscription is null)
            {
                return new StripeAPIResultModel(false, "error");
            }

            if (!subscription.IsActive)
            {
                return new StripeAPIResultModel(false, "Subscription already cancelled.");
            }

            var options = new SubscriptionUpdateOptions { CancelAtPeriodEnd = true };
            var service = new SubscriptionService();
            await service.UpdateAsync(subscription.Id, options);

            subscription.IsActive = false;
            _context.Subscriptions.Update(subscription);

            return new StripeAPIResultModel(true, "Subscription has been cancelled.");
        }

        private async Task<string> CheckIfCustomerExistsOrCreateOneAsync(StripeBillingRequest subReq, string userId)
        {
            var existingCustomer = await _context.StripeCustomer.FirstOrDefaultAsync(x => x.UserId == userId);
            if (existingCustomer == null)
            {
                return await CreateCustomerAsync(subReq, userId);
            }
            if (existingCustomer.PaymentMethod != subReq.PaymentMethod)
            {
                existingCustomer.PaymentMethod = subReq.PaymentMethod;
                await _context.SaveChangesAsync();
            }
            return existingCustomer.Id;
        }

        private async Task<string> CreateCustomerAsync(StripeBillingRequest subReq, string userId)
        {
            var options = new CustomerCreateOptions
            {
                Email = subReq.BillingEmail,
                Name = subReq.BillingName,
            };

            var service = new CustomerService();
            var customer = await service.CreateAsync(options);

            var stripeCustomerDto = new StripeCustomerDto
            {
                Id = customer.Id,
                UserId = userId,
                CustomerName = subReq.BillingName,
                BillingEmail = subReq.BillingEmail,
                PaymentMethod = subReq.PaymentMethod
            };

            var stripeCustomer = _mapper.Map<StripeCustomersEntity>(stripeCustomerDto);

            await _context.AddAsync(stripeCustomer);

            if (!(await _context.SaveChangesAsync() > 0))
            {
                return "error";
            }

            return customer.Id;
        }

        private static async Task UpdateCustomersDefaultPaymentIDAsync(string customerID, string PaymentMethod)
        {
            var options = new PaymentMethodAttachOptions
            {
                Customer = customerID,
            };
            var service = new PaymentMethodService();
            var paymentMethod = await service.AttachAsync(PaymentMethod, options);
            var customerOptions = new CustomerUpdateOptions
            {
                InvoiceSettings = new CustomerInvoiceSettingsOptions
                {
                    DefaultPaymentMethod = paymentMethod.Id,
                },
            };
            var customerService = new CustomerService();
            await customerService.UpdateAsync(customerID, customerOptions);
        }

        private async Task<StripeAPIResultModel> CreateSubscriptionAsync(string userId, string customerID, StripeBillingRequest subReq)
        {
            StripeAPIResultModel result = new(true);
            var subscriptionOptions = new SubscriptionCreateOptions
            {
                Customer = customerID,
                Items = new List<SubscriptionItemOptions>
                {
                    new SubscriptionItemOptions
                    {
                        Price = subReq.Price,
                    },
                },
            };

            // Check for Free Trial
            //if (subReq.FreeTrial)
            //{
            //    subscriptionOptions.TrialEnd = DateTime.UtcNow.AddDays(30);
            //}

            subscriptionOptions.AddExpand("latest_invoice.payment_intent");
            var subscriptionService = new SubscriptionService();
            try
            {
                Subscription subscription = await subscriptionService.CreateAsync(subscriptionOptions);

                var subscriptionDto = new SubscriptionsDto
                {
                    Id = subscription.Id,
                    UserId = userId,
                    StripeCustomerId = customerID,
                    Created = subscription.Created,
                    Currency = subscription.Currency,
                    CurrentPeriodEnd = subscription.CurrentPeriodEnd,
                    CurrentPeriodStart = subscription.CurrentPeriodStart,
                    LastInvoiceId = subscription.LatestInvoiceId,
                    TrialEnd = subscription.TrialEnd,
                    TrialStart = subscription.TrialStart,
                    IsActive = true
                };

                var subscriptionsEntity = _mapper.Map<SubscriptionsEntity>(subscriptionDto);

                await _context.AddAsync(subscriptionsEntity);
                await _context.SaveChangesAsync();

                result.Message = JsonConvert.SerializeObject(subscriptionsEntity);
            }
            catch (StripeException e)
            {
                Console.WriteLine($"Failed to create subscription.{e}");
                result.Success = false;
                result.Message = "Error processing your payment.";
            }

            return result;
        }
    }
}
