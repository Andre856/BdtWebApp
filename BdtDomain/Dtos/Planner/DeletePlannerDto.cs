using BdtShared.Dtos;

namespace BdtShared.Dtos.Planner;

public class DeletePlannerDto : IBaseDto<Guid>
{
    public Guid Id { get; set; }
}

