namespace Bdt.Shared.Dtos.Planner;

public class CreatePlannerDto : IUserDto
{
    public string UserId { get; set; } = Guid.Empty.ToString();
    public int WeekDayId { get; set; }
    public int WorkoutTypeId { get; set; }
    public decimal WorkoutDuration { get; set; }
}
