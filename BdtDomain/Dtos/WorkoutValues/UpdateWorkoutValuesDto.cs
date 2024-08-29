using BdtShared.Dtos;

namespace BdtShared.Dtos.WorkoutValues;

public class UpdateWorkoutValuesDto : IBaseDto<Guid>
{
    public Guid Id { get; set; }
    public int WorkoutTypeId { get; set; }
    public int Amount { get; set; }
}
