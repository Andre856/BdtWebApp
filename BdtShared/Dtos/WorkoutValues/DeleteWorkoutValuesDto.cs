using BdtShared.Dtos;

namespace BdtShared.Dtos.WorkoutValues;

public class DeleteWorkoutValuesDto : IBaseDto<Guid>
{
    public Guid Id { get; set; }
}
