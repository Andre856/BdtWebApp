using BdtApi.Application.Services.Generic;
using BdtApi.Domain.Entities;
using BdtShared.Dtos.Planner;

namespace BdtApi.Application.Services.Planner;

public interface IPlannerService : IGenericService<Guid, PlannerEntity, PlannerDto, UpdatePlannerDto, CreatePlannerDto, DeletePlannerDto>
{
    Task<IEnumerable<PlannerDto>?> GetAllByUserId(string userId);
}
