using AutoMapper;
using BdtApplication.ApiServices.Generic;
using BdtDomain.Dtos.Workouts;
using BdtDomain.Entities;
using BdtDomain.Repository;
using BdtInfrastructure.Context;

namespace BdtApplication.ApiServices.Workout;

public class WorkoutServiceApi : GenericService<Guid, WorkoutEntity, WorkoutDto, UpdateWorkoutDto, CreateWorkoutDto, DeleteWorkoutDto>, IWorkoutServiceApi
{
    public readonly BdtDbContext _context;
    public WorkoutServiceApi(IDeleteRepository<Guid, WorkoutEntity> repository,
        BdtDbContext dbContext, IMapper mapper)
        : base(repository, mapper)
    {
        _context = dbContext;
    }

    public async Task<IEnumerable<WorkoutDto>?> GetAllByUserId(string userId)
    {
        var entities = await _repository.GetAllByExpressionWithIncludes(x => x.UserId == userId);

        var dtos = _mapper.Map<IEnumerable<WorkoutDto>>(entities);

        return dtos;
    }

    public async Task<IEnumerable<WorkoutDto>?> GetLastMonthByUserId(string userId)
    {
        var entities = await _repository.GetAllByExpressionWithIncludes(x => x.UserId == userId && x.Date >= DateTime.Now.AddMonths(-1));

        var dtos = _mapper.Map<IEnumerable<WorkoutDto>>(entities);

        return dtos;
    }
}
