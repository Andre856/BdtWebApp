using BdtApplication.AppServices.GenericApi;
using BdtDomain.Dtos.WorkoutType;
using BdtDomain.Models.App;

namespace BdtApplication.AppServices.WorkoutType;

public interface IWorkoutTypeService : IGenericApiService
{
    Task<ApiWrapper<IEnumerable<WorkoutTypeDto>?>> GetAllWorkoutTypes();
}
