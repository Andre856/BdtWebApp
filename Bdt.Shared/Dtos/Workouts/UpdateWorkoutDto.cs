using Bdt.Shared.Dtos;
using Bdt.Shared.Dtos.WorkoutValues;

namespace Bdt.Shared.Dtos.Workouts;

public class UpdateWorkoutDto : IBaseDto<Guid>
{
    public Guid Id { get; set; }
    public int WorkoutTypeId { get; set; }
    public DateTime Date { get; set; }
    public decimal WorkoutTime { get; set; }
    public string? Comment { get; set; }
    public ICollection<WorkoutValuesDto> WokoutValues { get; set; }
}
