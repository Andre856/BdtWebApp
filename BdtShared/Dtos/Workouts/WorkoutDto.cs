using BdtShared.Dtos.WorkoutType;
using BdtShared.Dtos.WorkoutValues;

namespace BdtShared.Dtos.Workouts;

public class WorkoutDto : IBaseDto<Guid>, IUserDto
{
    public Guid Id { get; set; }
    public string UserId { get; set; }
    public int WorkoutTypeId { get; set; }
    public DateTime Date { get; set; }
    public decimal WorkoutTime { get; set; }
    public string? Comment { get; set; }
    public WorkoutTypeDto WorkoutType { get; set; }
    public ICollection<WorkoutValuesDto> WokoutValues { get; set; }
}
