using BdtClient.AppServices.GenericApi;
using Bdt.Shared.Dtos.WorkoutType;
using Bdt.Shared.Models.App;

namespace BdtClient.AppServices.WorkoutType;

public interface IWorkoutTypeService : IGenericApiService
{
    Task<ApiWrapper<IEnumerable<WorkoutTypeDto>?>> GetAllWorkoutTypes();
}
