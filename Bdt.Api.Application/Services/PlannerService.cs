using AutoMapper;
using Bdt.Api.Application.Services.Interfaces;
using Bdt.Api.Domain.Entities;
using Bdt.Api.Infrastructure.Repositories.Interfaces;
using Bdt.Shared.Dtos.Planner;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace Bdt.Api.Application.Services;

public class PlannerService : GenericService<Guid, PlannerEntity, PlannerDto, UpdatePlannerDto, CreatePlannerDto, DeletePlannerDto>, IPlannerService
{
    public PlannerService(IDeleteRepository<Guid, PlannerEntity> repository, IMapper mapper, ILogger<PlannerService> logger)
        : base(repository, mapper, logger) { }

    public async Task<IEnumerable<PlannerDto>?> GetAllByUserId(string userId)
    {
        try
        {
            Expression<Func<PlannerEntity, bool>> toCheck = x => x.UserId.Equals(userId);

            var result = await _repository.GetAllByExpressionWithIncludes(toCheck);

            return _mapper.Map<IEnumerable<PlannerDto>>(result);
        }
        catch (Exception ex)
        {
            throw BuildExceptionToThrow(ex);
        }
    }
}
