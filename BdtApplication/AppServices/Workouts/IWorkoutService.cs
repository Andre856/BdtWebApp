using BdtDomain.Dtos.Workouts;
using BdtDomain.Models.App;

namespace BdtApplication.AppServices.Workouts;

public interface IWorkoutService
{
    Task<ApiWrapper<IEnumerable<WorkoutDto>>> GetUserWorkouts();
    Task<ApiWrapper<WorkoutDto>> CreateUserWorkout(CreateWorkoutDto createWorkout);
    Task<ApiWrapper<IEnumerable<WorkoutDto>>> GetUserWorkoutsLastMonth();
    Task<ApiWrapper<WorkoutDto>> DeleteUserWorkout(Guid id);
}
