namespace Bdt.Shared.Dtos.BdtProduct;

public class BdtProductDto : IBaseDto<string>
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int Price { get; set; }
    public int PricePerMonth { get; set; }
    public string BillingInterval { get; set; }
    public string StripeTestApiId { get; set; }
    public string StripeProdApiId { get; set; }
}
