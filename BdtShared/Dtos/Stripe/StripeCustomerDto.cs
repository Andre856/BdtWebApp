namespace BdtShared.Dtos.Stripe;

public class StripeCustomerDto
{
    public string Id { get; set; }
    public string UserId { get; set; }
    public string CustomerName { get; set; }
    public string BillingEmail { get; set; }
    public string PaymentMethod { get; set; }
}
