namespace Bdt.Shared.Models.Timer;

public class TimerData
{
    public required string TimerType { get; set; }
    public required decimal TimerValue { get; set; }
    public required decimal TotalTime { get; set; }
    public decimal TimerPercentage { get => 100 * (TimerValue / TotalTime); }
}
