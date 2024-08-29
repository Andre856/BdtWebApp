using BdtApplication.ApiServices.Generic;
using BdtDomain.Dtos.WorkoutValues;
using BdtDomain.Entities;

namespace BdtApplication.ApiServices.WorkoutValues;

public interface IWorkoutValuesServiceApi : IGenericService<Guid, WorkoutValuesEntity, WorkoutValuesDto, UpdateWorkoutValuesDto, CreateWorkoutValuesDto, DeleteWorkoutValuesDto>
{

}
