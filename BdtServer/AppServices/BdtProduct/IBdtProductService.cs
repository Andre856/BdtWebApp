using BdtServer.AppServices.GenericApi;
using BdtShared.Dtos.BdtProduct;
using BdtShared.Models.App;

namespace BdtServer.AppServices.BdtProduct;

public interface IBdtProductService : IGenericApiService
{
    Task<ApiWrapper<IEnumerable<BdtProductDto>?>> GetAllProducts();
}
