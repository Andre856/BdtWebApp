namespace Bdt.Shared.Models.Workout;

public class WorkoutSummary
{
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
    public decimal TotalWorkoutTime { get; set; }
    public Dictionary<string, int> WorkoutCounts { get; set; }
}
