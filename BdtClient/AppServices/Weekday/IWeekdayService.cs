using BdtClient.AppServices.GenericApi;
using BdtShared.Dtos.WeekDay;
using BdtShared.Models.App;

namespace BdtClient.AppServices.Weekday;

public interface IWeekdayService : IGenericApiService
{
    Task<ApiWrapper<IEnumerable<WeekdayDto>?>> GetAllWeekdays();
}
