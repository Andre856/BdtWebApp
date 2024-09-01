using Bdt.Shared.Dtos.Planner;
using Bdt.Shared.Models.App;

namespace Bdt.Client.AppServices.Planner;

public interface IPlannerService
{
    Task<ApiWrapper<IEnumerable<PlannerDto>?>> GetUserPlans();
    Task<ApiWrapper<PlannerDto>> CreateUserPlans(CreatePlannerDto createPlans);
    Task<ApiWrapper<PlannerDto>> DeleteUserPlans(Guid id);
    Task<ApiWrapper<PlannerDto>> UpdateUserPlans(UpdatePlannerDto updatePlans);
}
