using BDtApi.ApiServices.Generic;
using BdtShared.Dtos.WorkoutValues;
using BdtShared.Entities;

namespace BDtApi.ApiServices.WorkoutValues;

public interface IWorkoutValuesService : IGenericService<Guid, WorkoutValuesEntity, WorkoutValuesDto, UpdateWorkoutValuesDto, CreateWorkoutValuesDto, DeleteWorkoutValuesDto>
{

}
