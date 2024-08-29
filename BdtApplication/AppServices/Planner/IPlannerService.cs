using BdtDomain.Dtos.Planner;
using BdtDomain.Models.App;

namespace BdtApplication.AppServices.Planner;

public interface IPlannerService
{
    Task<ApiWrapper<IEnumerable<PlannerDto>?>> GetUserPlans();
    Task<ApiWrapper<PlannerDto>> CreateUserPlans(CreatePlannerDto createPlans);
    Task<ApiWrapper<PlannerDto>> DeleteUserPlans(Guid id);
    Task<ApiWrapper<PlannerDto>> UpdateUserPlans(UpdatePlannerDto updatePlans);
}
