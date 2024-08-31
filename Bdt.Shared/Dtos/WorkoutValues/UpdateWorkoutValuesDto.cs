using Bdt.Shared.Dtos;

namespace Bdt.Shared.Dtos.WorkoutValues;

public class UpdateWorkoutValuesDto : IBaseDto<Guid>
{
    public Guid Id { get; set; }
    public int WorkoutTypeId { get; set; }
    public int Amount { get; set; }
}
