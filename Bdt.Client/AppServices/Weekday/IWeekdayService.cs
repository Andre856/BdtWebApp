using Bdt.Shared.Dtos.WeekDay;
using Bdt.Shared.Models.App;
using Bdt.Client.AppServices.GenericApi;

namespace Bdt.Client.AppServices.Weekday;

public interface IWeekdayService : IGenericApiService
{
    Task<ApiWrapper<IEnumerable<WeekdayDto>?>> GetAllWeekdays();
}
