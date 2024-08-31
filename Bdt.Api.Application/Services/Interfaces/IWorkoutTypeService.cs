using Bdt.Api.Domain.Entities;
using Bdt.Shared.Dtos.WorkoutType;

namespace Bdt.Api.Application.Services.Interfaces;

public interface IWorkoutTypeService : IGenericService<int, WorkoutTypeEntity, WorkoutTypeDto>
{

}
