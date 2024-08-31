using Bdt.Shared.Dtos.WorkoutType;
using Bdt.Shared.Models.App;

namespace Bdt.Shared.Static;

public static class StaticWorkoutTypes
{
    public static ApiWrapper<IEnumerable<WorkoutTypeDto>> WorkoutTypeContent { get; set; }
}