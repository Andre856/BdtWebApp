using AutoMapper;
using Bdt.Api.Application.Services.Interfaces;
using Bdt.Api.Infrastructure.Repositories.Interfaces;
using Bdt.Api.Domain.Entities;
using Bdt.Shared.Dtos.WorkoutValues;
using Microsoft.Extensions.Logging;

namespace Bdt.Api.Application.Services;

public class WorkoutValuesService : GenericService<Guid, WorkoutValuesEntity, WorkoutValuesDto, UpdateWorkoutValuesDto, CreateWorkoutValuesDto, DeleteWorkoutValuesDto>, IWorkoutValuesService
{
    public WorkoutValuesService(IDeleteRepository<Guid, WorkoutValuesEntity> repository, IMapper mapper, ILogger<WorkoutValuesService> logger)
        : base(repository, mapper, logger)
    { }
}
