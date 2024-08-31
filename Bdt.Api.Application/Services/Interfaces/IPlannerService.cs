using Bdt.Api.Domain.Entities;
using Bdt.Shared.Dtos.Planner;

namespace Bdt.Api.Application.Services.Interfaces;

public interface IPlannerService : IGenericService<Guid, PlannerEntity, PlannerDto, UpdatePlannerDto, CreatePlannerDto, DeletePlannerDto>
{
    Task<IEnumerable<PlannerDto>?> GetAllByUserId(string userId);
}
