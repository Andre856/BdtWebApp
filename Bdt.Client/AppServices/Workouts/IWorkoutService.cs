using Bdt.Shared.Dtos.Workouts;
using Bdt.Shared.Models.App;

namespace Bdt.Client.AppServices.Workouts;

public interface IWorkoutService
{
    Task<ApiWrapper<IEnumerable<WorkoutDto>>> GetUserWorkouts();
    Task<ApiWrapper<WorkoutDto>> CreateUserWorkout(CreateWorkoutDto createWorkout);
    Task<ApiWrapper<IEnumerable<WorkoutDto>>> GetUserWorkoutsLastMonth();
    Task<ApiWrapper<WorkoutDto>> DeleteUserWorkout(Guid id);
}
