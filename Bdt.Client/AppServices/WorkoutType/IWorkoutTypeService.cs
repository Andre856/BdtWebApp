using Bdt.Shared.Dtos.WorkoutType;
using Bdt.Shared.Models.App;
using Bdt.Client.AppServices.GenericApi;

namespace Bdt.Client.AppServices.WorkoutType;

public interface IWorkoutTypeService : IGenericApiService
{
    Task<ApiWrapper<IEnumerable<WorkoutTypeDto>?>> GetAllWorkoutTypes();
}
