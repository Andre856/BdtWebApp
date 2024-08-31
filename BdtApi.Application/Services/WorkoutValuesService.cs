using AutoMapper;
using BdtApi.Application.Services.Interfaces;
using BdtApi.Domain.Entities;
using BdtApi.Infrastructure.Repositories.Interfaces;
using BdtShared.Dtos.WorkoutValues;

namespace BdtApi.Application.Services;

public class WorkoutValuesService : GenericService<Guid, WorkoutValuesEntity, WorkoutValuesDto, UpdateWorkoutValuesDto, CreateWorkoutValuesDto, DeleteWorkoutValuesDto>, IWorkoutValuesService
{
    public WorkoutValuesService(IDeleteRepository<Guid, WorkoutValuesEntity> repository, IMapper mapper)
        : base(repository, mapper)
    { }
}
