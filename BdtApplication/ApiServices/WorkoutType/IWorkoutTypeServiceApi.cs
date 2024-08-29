using BdtApplication.ApiServices.Generic;
using BdtDomain.Dtos.WorkoutType;
using BdtDomain.Entities;

namespace BdtApplication.ApiServices.WorkoutType;

public interface IWorkoutTypeServiceApi : IGenericService<int, WorkoutTypeEntity, WorkoutTypeDto>
{

}
