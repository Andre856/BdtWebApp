using BdtShared.Dtos;
using BdtShared.Dtos.WorkoutValues;

namespace BdtShared.Dtos.Workouts;

public class UpdateWorkoutDto : IBaseDto<Guid>
{
    public Guid Id { get; set; }
    public int WorkoutTypeId { get; set; }
    public DateTime Date { get; set; }
    public decimal WorkoutTime { get; set; }
    public string? Comment { get; set; }
    public ICollection<WorkoutValuesDto> WokoutValues { get; set; }
}
