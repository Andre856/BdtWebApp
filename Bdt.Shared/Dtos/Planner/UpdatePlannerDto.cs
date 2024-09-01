namespace Bdt.Shared.Dtos.Planner;

public class UpdatePlannerDto : IBaseDto<Guid>
{
    public Guid Id { get; set; }
    public int WeekDayId { get; set; }
    public int WorkoutTypeId { get; set; }
    public decimal WorkoutDuration { get; set; }
}
