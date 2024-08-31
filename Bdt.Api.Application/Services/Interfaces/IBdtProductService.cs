using Bdt.Api.Domain.Entities;
using Bdt.Shared.Dtos.BdtProduct;

namespace Bdt.Api.Application.Services.Interfaces;

public interface IBdtProductService : IGenericService<string, BdtProductEntity, BdtProductDto>
{

}
