using Bdt.Shared.Dtos;

namespace Bdt.Shared.Dtos.Workouts;

public class DeleteWorkoutDto : IBaseDto<Guid>
{
    public Guid Id { get; set; }
}
