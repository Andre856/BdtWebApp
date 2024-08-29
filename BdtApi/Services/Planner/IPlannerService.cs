using BDtApi.ApiServices.Generic;
using BdtShared.Dtos.Planner;
using BdtShared.Entities;

namespace BDtApi.ApiServices.Planner;

public interface IPlannerService : IGenericService<Guid, PlannerEntity, PlannerDto, UpdatePlannerDto, CreatePlannerDto, DeletePlannerDto>
{
    Task<IEnumerable<PlannerDto>?> GetAllByUserId(string userId);
}
