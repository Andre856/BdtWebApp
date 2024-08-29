using BdtShared.Dtos.BdtProduct;
using BdtShared.Models.App;

namespace BdtShared.Static;

public static class StaticBdtProducts
{
    public static ApiWrapper<IEnumerable<BdtProductDto>> BdtProductsContent { get; set; }
}
