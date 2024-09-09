using AutoMapper;
using Bdt.Api.Application.Services.Interfaces;
using Bdt.Api.Infrastructure.Repositories.Interfaces;
using Bdt.Api.Domain.Entities;
using Bdt.Shared.Dtos.WorkoutType;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace Bdt.Api.Application.Services;

public class WorkoutTypeService : GenericService<int, WorkoutTypeEntity, WorkoutTypeDto>, IWorkoutTypeService
{
    private const string WorkoutTypeCache = "WorkoutTypeCache";
    private readonly IMemoryCache _cache;

    public WorkoutTypeService(IReadRepository<int, WorkoutTypeEntity> repository, IMemoryCache cache, IMapper mapper, ILogger<WorkoutTypeService> logger)
        : base(repository, mapper, logger)
    {
        _cache = cache;
    }

    public override async Task<IEnumerable<WorkoutTypeDto>> GetAllAsync()
    {
        try
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
        catch (Exception ex)
        {
            throw BuildExceptionToThrow(ex);
        }
    }
}
