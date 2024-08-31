using BdtClient.AppServices.GenericApi;
using Bdt.Shared.Dtos.BdtProduct;
using Bdt.Shared.Models.App;

namespace BdtClient.AppServices.BdtProduct;

public interface IBdtProductService : IGenericApiService
{
    Task<ApiWrapper<IEnumerable<BdtProductDto>?>> GetAllProducts();
}
