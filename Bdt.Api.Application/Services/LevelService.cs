using AutoMapper;
using Bdt.Api.Application.Services.Interfaces;
using Bdt.Api.Infrastructure.Repositories.Interfaces;
using Bdt.Api.Domain.Entities;
using Bdt.Shared.Dtos.Levels;
using Microsoft.Extensions.Caching.Memory;

namespace Bdt.Api.Application.Services
{
    public class LevelService : GenericService<int, LevelEntity, LevelDto>, ILevelService
    {
        private const string WorkoutTypeCache = "WorkoutTypeCache";
        private readonly IMemoryCache _cache;

        public LevelService(IReadRepository<int, LevelEntity> repository, IMemoryCache cache, IMapper mapper)
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
