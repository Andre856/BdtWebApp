using AutoMapper;
using BdtApplication.ApiServices.Generic;
using BdtDomain.Dtos.Planner;
using BdtDomain.Entities;
using BdtDomain.Repository;
using System.Linq.Expressions;

namespace BdtApplication.ApiServices.Planner;

public class PlannerServiceApi : GenericService<Guid, PlannerEntity, PlannerDto, UpdatePlannerDto, CreatePlannerDto, DeletePlannerDto>, IPlannerServiceApi
{
    public PlannerServiceApi(IDeleteRepository<Guid, PlannerEntity> repository, IMapper mapper)
        : base(repository, mapper) { }

    public async Task<IEnumerable<PlannerDto>?> GetAllByUserId(string userId)
    {
        Expression<Func<PlannerEntity, bool>> toCheck = x => x.UserId.Equals(userId);

        var result = await _repository.GetAllByExpressionWithIncludes(toCheck);

        return _mapper.Map<IEnumerable<PlannerDto>>(result);
    }
}
