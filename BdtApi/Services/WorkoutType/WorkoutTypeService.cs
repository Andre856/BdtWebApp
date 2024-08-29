using AutoMapper;
using BdtApi.Repository;
using BDtApi.ApiServices.Generic;
using BdtShared.Dtos.WorkoutType;
using BdtShared.Entities;
using Microsoft.Extensions.Caching.Memory;

namespace BDtApi.ApiServices.WorkoutType;

public class WorkoutTypeService : GenericService<int, WorkoutTypeEntity, WorkoutTypeDto>, IWorkoutTypeService
{
    private const string WorkoutTypeCache = "WorkoutTypeCache";
    private readonly IMemoryCache _cache;

    public WorkoutTypeService(IReadRepository<int, WorkoutTypeEntity> repository, IMemoryCache cache, IMapper mapper)
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
