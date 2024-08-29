using BdtApplication.ApiServices.Generic;
using BdtDomain.Dtos.Workouts;
using BdtDomain.Entities;

namespace BdtApplication.ApiServices.Workout;

public interface IWorkoutServiceApi : IGenericService<Guid, WorkoutEntity, WorkoutDto>
{
    Task<IEnumerable<WorkoutDto>?> GetAllByUserId(string userId);
    Task<IEnumerable<WorkoutDto>?> GetLastMonthByUserId(string userId);
}
