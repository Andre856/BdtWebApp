using Bdt.Shared.Dtos.Planner;
using Bdt.Shared.Models.App;
using Bdt.Shared.Models.Charts;
using Plugin.LocalNotification;

namespace BdtClient.Helpers;

public static class Calculations
{
    private const int _roundTo = 0;

    public static DateDataItem[] CalculateMovingAverage(DateDataItem[] data, int windowSize)
    {
        DateDataItem[] movingAverages = new DateDataItem[data.Length];

        for (int i = 0; i < data.Length; i++)
        {
            double sum = 0;
            int count = 0;

            for (int j = Math.Max(0, i - windowSize + 1); j <= i; j++)
            {
                sum += data[j].yAxis;
                count++;
            }

            double average = Math.Round(sum / count, _roundTo);

            movingAverages[i] = new DateDataItem
            {
                xAxis = data[i].xAxis,
                yAxis = average
            };
        }

        return movingAverages;
    }

    public static void SetNotifications(IEnumerable<PlannerDto> weeklyPlans, TimeSpan notificationTime)
    {
        LocalNotificationCenter.Current.Clear();
        var now = DateTime.Now;
        
        foreach (var plan in weeklyPlans)
        {
            int daysUntilNextWeekday = (int)now.DayOfWeek - plan.WeekDayId;
            daysUntilNextWeekday = daysUntilNextWeekday < 0 ? 6 + daysUntilNextWeekday : daysUntilNextWeekday;
            var notifyTime = now.AddDays(daysUntilNextWeekday).Date.Add(notificationTime);

            var notificationRequest = new NotificationRequest
            {
                NotificationId = plan.WeekDayId,
                Title = $"You have a workout scheduled for today",
                Subtitle = "Busy Dad Training",
                Description = $"{plan.WorkoutType.Name} for ({plan.WorkoutDuration} minutes)",
                Schedule = new NotificationRequestSchedule
                {
                    NotifyTime = notifyTime,
                    NotifyRepeatInterval = TimeSpan.FromDays(7)
                }
            };

            LocalNotificationCenter.Current.Show(notificationRequest);
        }
    }

    //public static BollingerItem[] CalculateBollingerBands(DataItem[] data, int period, double standardDeviations)
    //{
    //    List<BollingerItem> bollingerBands = new List<BollingerItem>();

    //    for (int i = period - 1; i < data.Length; i++)
    //    {
    //        // Calculate the average for the current period
    //        double sum = 0;
    //        for (int j = i; j > i - period; j--)
    //        {
    //            sum += data[j].yAxis;
    //        }
    //        double average = sum / period;

    //        // Calculate the standard deviation for the current period
    //        double varianceSum = 0;
    //        for (int j = i; j > i - period; j--)
    //        {
    //            double deviation = data[j].yAxis - average;
    //            varianceSum += deviation * deviation;
    //        }
    //        double standardDeviation = Math.Sqrt(varianceSum / period);

    //        // Calculate the Bollinger Band values
    //        double upperBand = Math.Round(average + standardDeviations * standardDeviation, _roundTo);
    //        double lowerBand = Math.Round(average - standardDeviations * standardDeviation, _roundTo);

    //        // Create a BollingerItem object and add it to the list
    //        BollingerItem bollingerItem = new BollingerItem
    //        {
    //            xAxis = data[i].xAxis,
    //            yAxisLower = lowerBand,
    //            yAxis = data[i].yAxis,
    //            yAxisUpper = upperBand
    //        };
    //        bollingerBands.Add(bollingerItem);
    //    }

    //    return bollingerBands.ToArray();
    //}

}
