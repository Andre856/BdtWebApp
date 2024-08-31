using Bdt.Shared.Dtos.WorkoutType;
using Bdt.Shared.Dtos.WorkoutValues;

namespace Bdt.Shared.Dtos.Workouts;

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
