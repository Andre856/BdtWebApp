using BDtApi.ApiServices.Generic;
using BdtShared.Dtos.WorkoutType;
using BdtShared.Entities;

namespace BDtApi.ApiServices.WorkoutType;

public interface IWorkoutTypeService : IGenericService<int, WorkoutTypeEntity, WorkoutTypeDto>
{

}
