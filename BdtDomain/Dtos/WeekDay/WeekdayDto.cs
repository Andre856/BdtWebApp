using BdtShared.Dtos;

namespace BdtShared.Dtos.WeekDay;

public class WeekdayDto : IBaseDto<int>
{
    public int Id { get; set; }
    public string Name { get; set; }
}
