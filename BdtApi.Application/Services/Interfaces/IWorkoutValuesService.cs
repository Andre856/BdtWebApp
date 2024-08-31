using BdtApi.Domain.Entities;
using BdtShared.Dtos.WorkoutValues;

namespace BdtApi.Application.Services.Interfaces;

public interface IWorkoutValuesService : IGenericService<Guid, WorkoutValuesEntity, WorkoutValuesDto, UpdateWorkoutValuesDto, CreateWorkoutValuesDto, DeleteWorkoutValuesDto>
{

}
