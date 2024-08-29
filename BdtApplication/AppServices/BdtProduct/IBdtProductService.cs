using BdtApplication.AppServices.GenericApi;
using BdtDomain.Dtos.BdtProduct;
using BdtDomain.Models.App;

namespace BdtApplication.AppServices.BdtProduct;

public interface IBdtProductService : IGenericApiService
{
    Task<ApiWrapper<IEnumerable<BdtProductDto>?>> GetAllProducts();
}
