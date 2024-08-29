using BdtClient.AppServices.GenericApi;
using BdtShared.Dtos.BdtProduct;
using BdtShared.Models.App;

namespace BdtClient.AppServices.BdtProduct;

public interface IBdtProductService : IGenericApiService
{
    Task<ApiWrapper<IEnumerable<BdtProductDto>?>> GetAllProducts();
}
