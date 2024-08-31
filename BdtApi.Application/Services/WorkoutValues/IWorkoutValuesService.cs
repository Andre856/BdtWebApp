using BdtApi.Application.Services.Generic;
using BdtApi.Domain.Entities;
using BdtShared.Dtos.WorkoutValues;

namespace BdtApi.Application.Services.WorkoutValues;

public interface IWorkoutValuesService : IGenericService<Guid, WorkoutValuesEntity, WorkoutValuesDto, UpdateWorkoutValuesDto, CreateWorkoutValuesDto, DeleteWorkoutValuesDto>
{

}
