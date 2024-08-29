using BdtClient.AppServices.GenericApi;
using BdtShared.Dtos.WorkoutType;
using BdtShared.Models.App;

namespace BdtClient.AppServices.WorkoutType;

public interface IWorkoutTypeService : IGenericApiService
{
    Task<ApiWrapper<IEnumerable<WorkoutTypeDto>?>> GetAllWorkoutTypes();
}
