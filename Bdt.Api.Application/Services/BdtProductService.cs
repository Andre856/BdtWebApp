using AutoMapper;
using Bdt.Api.Application.Services.Interfaces;
using Bdt.Api.Infrastructure.Repositories.Interfaces;
using Bdt.Api.Domain.Entities;
using Bdt.Shared.Dtos.BdtProduct;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace Bdt.Api.Application.Services;

public class BdtProductService : GenericService<string, BdtProductEntity, BdtProductDto>, IBdtProductService
{
    private const string BdtProductCache = "BdtProductCache";
    private readonly IMemoryCache _cache;

    public BdtProductService(IReadRepository<string, BdtProductEntity> repository, IMemoryCache cache, IMapper mapper, ILogger<BdtProductService> logger)
        : base(repository, mapper, logger)
    {
        _cache = cache;
    }

    public override async Task<IEnumerable<BdtProductDto>> GetAllAsync()
    {
        try
        {
            if (!_cache.TryGetValue(BdtProductCache, out IEnumerable<BdtProductEntity>? products))
            {
                products = await _repository.GetAllAsync()
                    ?? throw new Exception("Could not get levels from database.");
                _cache.Set(BdtProductCache, products, TimeSpan.FromDays(1));
            }

            if (products is null)
                return Enumerable.Empty<BdtProductDto>();

            return _mapper.Map<IEnumerable<BdtProductDto>>(products);
        }
        catch (Exception ex)
        {
            throw BuildExceptionToThrow(ex);
        }
    }
}
