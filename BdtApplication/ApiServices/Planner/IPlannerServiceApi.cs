using BdtDomain.Entities;
using BdtApplication.ApiServices.Generic;
using BdtDomain.Dtos.Planner;

namespace BdtApplication.ApiServices.Planner;

public interface IPlannerServiceApi : IGenericService<Guid, PlannerEntity, PlannerDto, UpdatePlannerDto, CreatePlannerDto, DeletePlannerDto>
{
    Task<IEnumerable<PlannerDto>?> GetAllByUserId(string userId);
}
