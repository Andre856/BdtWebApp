using Bdt.Shared.Dtos.BdtProduct;
using Bdt.Shared.Models.App;

namespace Bdt.Shared.Static;

public static class StaticBdtProducts
{
    public static ApiWrapper<IEnumerable<BdtProductDto>> BdtProductsContent { get; set; }
}
