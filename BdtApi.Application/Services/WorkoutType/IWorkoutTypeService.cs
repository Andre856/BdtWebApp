using BdtApi.Application.Services.Generic;
using BdtApi.Domain.Entities;
using BdtShared.Dtos.WorkoutType;

namespace BdtApi.Application.Services.WorkoutType;

public interface IWorkoutTypeService : IGenericService<int, WorkoutTypeEntity, WorkoutTypeDto>
{

}
