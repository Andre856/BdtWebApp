using Bdt.Shared.Dtos.WorkoutValues;

namespace Bdt.Shared.Dtos.Workouts;

public class CreateWorkoutDto : IUserDto
{
    public string UserId { get; set; } = Guid.Empty.ToString();
    public int WorkoutTypeId { get; set; }
    public DateTime Date { get; set; }
    public decimal WorkoutTime { get; set; }
    public string? Comment { get; set; }
    public ICollection<CreateWorkoutValuesDto> WokoutValues { get; set; }
}
