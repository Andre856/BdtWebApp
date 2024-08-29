using BDtApi.ApiServices.Generic;
using BdtShared.Dtos.Workouts;
using BdtShared.Entities;

namespace BDtApi.ApiServices.Workout;

public interface IWorkoutService : IGenericService<Guid, WorkoutEntity, WorkoutDto>
{
    Task<IEnumerable<WorkoutDto>?> GetAllByUserId(string userId);
    Task<IEnumerable<WorkoutDto>?> GetLastMonthByUserId(string userId);
}
