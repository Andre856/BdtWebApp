using Bdt.Shared.Dtos;

namespace Bdt.Shared.Dtos.WeekDay;

public class WeekdayDto : IBaseDto<int>
{
    public int Id { get; set; }
    public string Name { get; set; }
}
