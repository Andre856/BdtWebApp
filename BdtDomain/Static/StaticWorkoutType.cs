using BdtShared.Dtos.WorkoutType;
using BdtShared.Models.App;

namespace BdtShared.Static;

public static class StaticWorkoutTypes
{
    public static ApiWrapper<IEnumerable<WorkoutTypeDto>> WorkoutTypeContent { get; set; }
}