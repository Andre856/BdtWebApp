using System.ComponentModel.DataAnnotations;

namespace BdtShared.Models.StripeModels;

public class StripeBillingRequest
{
    [Required(ErrorMessage = "Please enter cardholders full name.")]
    public string BillingName { get; set; }

    [Required(ErrorMessage = "Please enter a valid email address.")]
    [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
    public string BillingEmail { get; set; }
    public string PaymentMethod { get; set; }
    public string Price { get; set; }

    public string? City { get; set; }
    public string? Line1 { get; set; }
    public string? Line2 { get; set; }

    [Required(ErrorMessage = "Please select your country.")]
    public string Country { get; set; }

    [Required(ErrorMessage = "Please enter your postal code.")]
    public string PostalCode { get; set; }
}
