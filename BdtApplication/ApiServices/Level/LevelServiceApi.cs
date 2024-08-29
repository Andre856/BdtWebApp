using BdtDomain.Entities;
using BdtApplication.ApiServices.Generic;
using BdtDomain.Dtos.Levels;
using Microsoft.Extensions.Caching.Memory;
using BdtDomain.Repository;
using AutoMapper;

namespace BdtApplication.ApiServices.Level
{
    public class LevelServiceApi : GenericService<int, LevelEntity, LevelDto>, ILevelServiceApi
    {
        private const string WorkoutTypeCache = "WorkoutTypeCache";
        private readonly IMemoryCache _cache;

        public LevelServiceApi(IReadRepository<int, LevelEntity> repository, IMemoryCache cache, IMapper mapper)
            : base(repository, mapper)
        {
            _cache = cache;
        }

        public override async Task<IEnumerable<LevelDto>> GetAllAsync()
        {
            if (!_cache.TryGetValue(WorkoutTypeCache, out IEnumerable<LevelEntity>? levels))
            {
                levels = await _repository.GetAllAsync()
                    ?? throw new Exception("Could not get levels from database.");
                _cache.Set(WorkoutTypeCache, levels, TimeSpan.FromDays(1));
            }

            if (levels is null)
                return Enumerable.Empty<LevelDto>();

            return _mapper.Map<IEnumerable<LevelDto>>(levels);
        }
    }
}
