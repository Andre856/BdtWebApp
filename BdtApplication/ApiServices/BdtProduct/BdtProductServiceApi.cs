using BdtDomain.Entities;
using BdtApplication.ApiServices.Generic;
using BdtDomain.Dtos.BdtProduct;
using Microsoft.Extensions.Caching.Memory;
using BdtDomain.Repository;
using AutoMapper;

namespace BdtApplication.ApiServices.BdtProduct
{
    public class BdtProductServiceApi : GenericService<string, BdtProductEntity, BdtProductDto>, IBdtProductServiceApi
    {
        private const string BdtProductCache = "BdtProductCache";
        private readonly IMemoryCache _cache;

        public BdtProductServiceApi(IReadRepository<string, BdtProductEntity> repository, IMemoryCache cache, IMapper mapper)
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
}
