namespace BdtShared.Dtos.WorkoutType;

public class WorkoutTypeDto : IBaseDto<int>
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int? PushUps { get; set; }
}
