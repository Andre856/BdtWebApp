using Bdt.Shared.Dtos.WeekDay;
using Bdt.Shared.Models.App;

namespace Bdt.Shared.Static;

public static class StaticWeekdays
{
    public static ApiWrapper<IEnumerable<WeekdayDto>> WeekdayContent { get; set; }
}