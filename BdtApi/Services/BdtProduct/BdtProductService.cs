using AutoMapper;
using BdtApi.Repository;
using BDtApi.ApiServices.Generic;
using BdtShared.Dtos.BdtProduct;
using BdtShared.Entities;
using Microsoft.Extensions.Caching.Memory;

namespace BDtApi.ApiServices.BdtProduct;

public class BdtProductService : GenericService<string, BdtProductEntity, BdtProductDto>, IBdtProductService
{
    private const string BdtProductCache = "BdtProductCache";
    private readonly IMemoryCache _cache;

    public BdtProductService(IReadRepository<string, BdtProductEntity> repository, IMemoryCache cache, IMapper mapper)
        : base(repository, mapper)
    {
        _cache = cache;
    }

    public override async Task<IEnumerable<BdtProductDto>> GetAllAsync()
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
}
