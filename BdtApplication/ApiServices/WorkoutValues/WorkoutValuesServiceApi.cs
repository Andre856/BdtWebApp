using AutoMapper;
using BdtApplication.ApiServices.Generic;
using BdtDomain.Dtos.WorkoutValues;
using BdtDomain.Entities;
using BdtDomain.Repository;

namespace BdtApplication.ApiServices.WorkoutValues;

public class WorkoutValuesServiceApi : GenericService<Guid, WorkoutValuesEntity, WorkoutValuesDto, UpdateWorkoutValuesDto, CreateWorkoutValuesDto, DeleteWorkoutValuesDto>, IWorkoutValuesServiceApi
{
    public WorkoutValuesServiceApi(IDeleteRepository<Guid, WorkoutValuesEntity> repository, IMapper mapper)
        : base(repository, mapper)
    { }
}
