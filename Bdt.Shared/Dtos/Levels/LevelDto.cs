namespace Bdt.Shared.Dtos.Levels;

public class LevelDto : IBaseDto<int>
{
    public int Id { get; set; }
    public string Level { get; set; }
    public int Lower6Count { get; set; }
    public int Upper6Count { get; set; }
    public int LowerNavySeal { get; set; }
    public int UpperNavySeal { get; set; }
}
