using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BdtShared.Entities;

[Table("BdtProduct")]
public class BdtProductEntity : IEntity<string>
{
    [Column("Id")]
    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString();

    [Column("Title")]
    [Required]
    public string Title { get; set; } = string.Empty;

    [Column("Description")]
    public string? Description { get; set; }

    [Column("Price")]
    [Required]
    public int Price { get; set; }

    [Column("PricePerMonth")]
    [Required]
    public int PricePerMonth { get; set; }

    [Column("Interval")]
    [Required]
    public string BillingInterval { get; set; }

    [Column("StripeTestApiId")]
    [Required]
    public string StripeTestApiId { get; set; }

    [Column("StripeProdApiId")]
    [Required]
    public string StripeProdApiId { get; set; }
}
