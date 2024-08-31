using BdtApi.Domain.Entities;
using BdtShared.Dtos.WorkoutType;

namespace BdtApi.Application.Services.Interfaces;

public interface IWorkoutTypeService : IGenericService<int, WorkoutTypeEntity, WorkoutTypeDto>
{

}
