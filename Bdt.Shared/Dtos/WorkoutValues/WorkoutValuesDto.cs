using Bdt.Shared.Dtos.WorkoutType;

namespace Bdt.Shared.Dtos.WorkoutValues;

public class WorkoutValuesDto : IBaseDto<Guid>
{
    public Guid Id { get; set; }
    public Guid WorkoutId { get; set; }
    public int WorkoutTypeId { get; set; }
    public int Amount { get; set; }
    public WorkoutTypeDto? WorkoutType { get; set; }
}
