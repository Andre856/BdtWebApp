using BdtApi.Application.Services.Generic;
using BdtApi.Domain.Entities;
using BdtShared.Dtos.Workouts;

namespace BdtApi.Application.Services.Workout;

public interface IWorkoutService : IGenericService<Guid, WorkoutEntity, WorkoutDto>
{
    Task<IEnumerable<WorkoutDto>?> GetAllByUserId(string userId);
    Task<IEnumerable<WorkoutDto>?> GetLastMonthByUserId(string userId);
}
