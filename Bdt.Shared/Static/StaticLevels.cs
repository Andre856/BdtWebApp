using Bdt.Shared.Dtos.Levels;
using Bdt.Shared.Models.App;

namespace Bdt.Shared.Static;

public static class StaticLevels
{
    public static ApiWrapper<IEnumerable<LevelDto>> LevelContent { get; set; }
}
