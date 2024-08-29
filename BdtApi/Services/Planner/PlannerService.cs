using AutoMapper;
using BdtApi.Repository;
using BDtApi.ApiServices.Generic;
using BdtShared.Dtos.Planner;
using BdtShared.Entities;
using System.Linq.Expressions;

namespace BDtApi.ApiServices.Planner;

public class PlannerService : GenericService<Guid, PlannerEntity, PlannerDto, UpdatePlannerDto, CreatePlannerDto, DeletePlannerDto>, IPlannerService
{
    public PlannerService(IDeleteRepository<Guid, PlannerEntity> repository, IMapper mapper)
        : base(repository, mapper) { }

    public async Task<IEnumerable<PlannerDto>?> GetAllByUserId(string userId)
    {
        Expression<Func<PlannerEntity, bool>> toCheck = x => x.UserId.Equals(userId);

        var result = await _repository.GetAllByExpressionWithIncludes(toCheck);

        return _mapper.Map<IEnumerable<PlannerDto>>(result);
    }
}
