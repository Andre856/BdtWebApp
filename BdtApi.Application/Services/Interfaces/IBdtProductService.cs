using BdtApi.Domain.Entities;
using BdtShared.Dtos.BdtProduct;

namespace BdtApi.Application.Services.Interfaces;

public interface IBdtProductService : IGenericService<string, BdtProductEntity, BdtProductDto>
{

}
