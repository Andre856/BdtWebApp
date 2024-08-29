using BdtServer.AppServices.GenericApi;
using BdtShared.Dtos.WorkoutType;
using BdtShared.Models.App;

namespace BdtServer.AppServices.WorkoutType;

public interface IWorkoutTypeService : IGenericApiService
{
    Task<ApiWrapper<IEnumerable<WorkoutTypeDto>?>> GetAllWorkoutTypes();
}
