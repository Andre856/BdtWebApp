using BdtShared.Dtos.Planner;
using BdtShared.Models.App;

namespace BdtServer.AppServices.Planner;

public interface IPlannerService
{
    Task<ApiWrapper<IEnumerable<PlannerDto>?>> GetUserPlans();
    Task<ApiWrapper<PlannerDto>> CreateUserPlans(CreatePlannerDto createPlans);
    Task<ApiWrapper<PlannerDto>> DeleteUserPlans(Guid id);
    Task<ApiWrapper<PlannerDto>> UpdateUserPlans(UpdatePlannerDto updatePlans);
}
