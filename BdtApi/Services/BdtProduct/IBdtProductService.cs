using BDtApi.ApiServices.Generic;
using BdtShared.Dtos.BdtProduct;
using BdtShared.Entities;

namespace BDtApi.ApiServices.BdtProduct;

public interface IBdtProductService : IGenericService<string, BdtProductEntity, BdtProductDto>
{

}
