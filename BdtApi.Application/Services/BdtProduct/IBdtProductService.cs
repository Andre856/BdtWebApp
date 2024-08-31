using BdtApi.Application.Services.Generic;
using BdtApi.Domain.Entities;
using BdtShared.Dtos.BdtProduct;

namespace BdtApi.Application.Services.BdtProduct;

public interface IBdtProductService : IGenericService<string, BdtProductEntity, BdtProductDto>
{

}
