using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BdtApi.Domain.Entities;

[Table("Subscriptions")]
public class SubscriptionsEntity : IEntity<string>
{
    [Key]
    [Column("Id")]
    public string Id { get; set; }

    [Column("UserId")]
    [Required]
    public string UserId { get; set; }

    [Column("StripeCustomerId")]
    [Required]
    public string StripeCustomerId { get; set; }

    [Column("Created")]
    [Required]
    public DateTime Created { get; set; }

    [Column("Currency")]
    [Required]
    public string Currency { get; set; }

    [Column("CurrentPeriodStart")]
    [Required]
    public DateTime CurrentPeriodStart { get; set; }

    [Column("CurrentPeriodEnd")]
    [Required]
    public DateTime CurrentPeriodEnd { get; set; }

    [Column("LastInvoiceId")]
    [Required]
    public string LastInvoiceId { get; set; }

    [Column("TrialStart")]
    public DateTime? TrialStart { get; set; }

    [Column("TrialEnd")]
    public DateTime? TrialEnd { get; set; }

    [Column("IsActive")]
    [Required]
    public bool IsActive { get; set; }

    [Column("CancellationRequested")]
    [Required]
    public bool CancellationRequested { get; set; }
}
