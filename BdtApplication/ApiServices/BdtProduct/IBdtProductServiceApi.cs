using BdtApplication.ApiServices.Generic;
using BdtDomain.Dtos.BdtProduct;
using BdtDomain.Entities;

namespace BdtApplication.ApiServices.BdtProduct;

public interface IBdtProductServiceApi : IGenericService<string, BdtProductEntity, BdtProductDto>
{

}
