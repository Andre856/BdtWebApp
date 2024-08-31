using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bdt.Api.Domain.Entities;

[Table("StripeCustomers")]
public class StripeCustomersEntity : IEntity<string>
{
    [Key]
    [Column("Id")]
    public string Id { get; set; }

    [Column("UserId")]
    [Required]
    public string UserId { get; set; }

    [Column("BillngName")]
    [Required]
    public string CustomerName { get; set; }

    [Column("BillingEmail")]
    [Required]
    public string BillingEmail { get; set; }

    [Column("PaymentMethod")]
    [Required]
    public string PaymentMethod { get; set; }

    [ForeignKey("UserId")]
    public UserEntity User { get; set; }
}
