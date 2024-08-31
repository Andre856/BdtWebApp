using BdtShared.Dtos;

namespace BdtShared.Dtos.Workouts;

public class DeleteWorkoutDto : IBaseDto<Guid>
{
    public Guid Id { get; set; }
}
