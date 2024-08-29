namespace BdtShared.Dtos.Stripe;

public class SubscriptionsDto
{
    public string Id { get; set; }
    public string UserId { get; set; }
    public string StripeCustomerId { get; set; }
    public DateTime Created { get; set; }
    public string Currency { get; set; }
    public DateTime CurrentPeriodStart { get; set; }
    public DateTime CurrentPeriodEnd { get; set; }
    public string LastInvoiceId { get; set; }
    public DateTime? TrialStart { get; set; }
    public DateTime? TrialEnd { get; set; }
    public bool IsActive { get; set; }
    public bool CancellationRequested { get; set; }
}
