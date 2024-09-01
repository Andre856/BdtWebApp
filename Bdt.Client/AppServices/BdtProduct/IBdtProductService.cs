using Bdt.Shared.Dtos.BdtProduct;
using Bdt.Shared.Models.App;
using Bdt.Client.AppServices.GenericApi;

namespace Bdt.Client.AppServices.BdtProduct;

public interface IBdtProductService : IGenericApiService
{
    Task<ApiWrapper<IEnumerable<BdtProductDto>?>> GetAllProducts();
}
