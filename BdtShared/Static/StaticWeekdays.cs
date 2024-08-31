using BdtShared.Dtos.WeekDay;
using BdtShared.Models.App;

namespace BdtShared.Static;

public static class StaticWeekdays
{
    public static ApiWrapper<IEnumerable<WeekdayDto>> WeekdayContent { get; set; }
}