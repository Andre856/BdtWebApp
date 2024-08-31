using Bdt.Api.Domain.Entities;
using Bdt.Shared.Dtos.WorkoutValues;

namespace Bdt.Api.Application.Services.Interfaces;

public interface IWorkoutValuesService : IGenericService<Guid, WorkoutValuesEntity, WorkoutValuesDto, UpdateWorkoutValuesDto, CreateWorkoutValuesDto, DeleteWorkoutValuesDto>
{

}
