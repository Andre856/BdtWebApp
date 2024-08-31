using Bdt.Api.Domain.Entities;
using Bdt.Shared.Dtos.Workouts;

namespace Bdt.Api.Application.Services.Interfaces;

public interface IWorkoutService : IGenericService<Guid, WorkoutEntity, WorkoutDto>
{
    Task<IEnumerable<WorkoutDto>?> GetAllByUserId(string userId);
    Task<IEnumerable<WorkoutDto>?> GetLastMonthByUserId(string userId);
}
