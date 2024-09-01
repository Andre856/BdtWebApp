namespace Bdt.Shared.Dtos.WorkoutValues;

public class DeleteWorkoutValuesDto : IBaseDto<Guid>
{
    public Guid Id { get; set; }
}
