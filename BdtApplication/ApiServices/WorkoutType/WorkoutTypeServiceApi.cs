using AutoMapper;
using BdtApplication.ApiServices.Generic;
using BdtDomain.Dtos.WorkoutType;
using BdtDomain.Entities;
using BdtDomain.Repository;
using Microsoft.Extensions.Caching.Memory;

namespace BdtApplication.ApiServices.WorkoutType
{
    public class WorkoutTypeServiceApi : GenericService<int, WorkoutTypeEntity, WorkoutTypeDto>, IWorkoutTypeServiceApi
    {
        private const string WorkoutTypeCache = "WorkoutTypeCache";
        private readonly IMemoryCache _cache;

        public WorkoutTypeServiceApi(IReadRepository<int, WorkoutTypeEntity> repository, IMemoryCache cache, IMapper mapper)
            : base(repository, mapper)
        {
            _cache = cache;
        }

        public override async Task<IEnumerable<WorkoutTypeDto>> GetAllAsync()
        {
            if (!_cache.TryGetValue(WorkoutTypeCache, out IEnumerable<WorkoutTypeEntity>? workoutTypes))
            {
                workoutTypes = await _repository.GetAllAsync();
                _cache.Set(WorkoutTypeCache, workoutTypes, TimeSpan.FromDays(1));
            }

            if (workoutTypes is null)
                return Enumerable.Empty<WorkoutTypeDto>();

            return _mapper.Map<IEnumerable<WorkoutTypeDto>>(workoutTypes);
        }
    }
}
