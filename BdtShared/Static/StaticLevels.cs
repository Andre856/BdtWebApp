using BdtShared.Dtos.Levels;
using BdtShared.Models.App;

namespace BdtShared.Static;

public static class StaticLevels
{
    public static ApiWrapper<IEnumerable<LevelDto>> LevelContent { get; set; }
}
