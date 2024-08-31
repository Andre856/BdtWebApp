using AutoMapper;
using Bdt.Api.Application.Services.Interfaces;
using Bdt.Api.Infrastructure.Context;
using Bdt.Api.Infrastructure.Repositories.Interfaces;
using Bdt.Api.Domain.Entities;
using Bdt.Shared.Dtos.Workouts;

namespace Bdt.Api.Application.Services;

public class WorkoutService : GenericService<Guid, WorkoutEntity, WorkoutDto, UpdateWorkoutDto, CreateWorkoutDto, DeleteWorkoutDto>, IWorkoutService
{
    public readonly BdtDbContext _context;
    public WorkoutService(IDeleteRepository<Guid, WorkoutEntity> repository,
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
