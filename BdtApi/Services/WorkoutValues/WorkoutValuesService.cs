using AutoMapper;
using BdtApi.Repository;
using BDtApi.ApiServices.Generic;
using BdtShared.Dtos.WorkoutValues;
using BdtShared.Entities;

namespace BDtApi.ApiServices.WorkoutValues;

public class WorkoutValuesService : GenericService<Guid, WorkoutValuesEntity, WorkoutValuesDto, UpdateWorkoutValuesDto, CreateWorkoutValuesDto, DeleteWorkoutValuesDto>, IWorkoutValuesService
{
    public WorkoutValuesService(IDeleteRepository<Guid, WorkoutValuesEntity> repository, IMapper mapper)
        : base(repository, mapper)
    { }
}
