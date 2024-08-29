using BdtServer.AppServices.GenericApi;
using BdtShared.Dtos.WeekDay;
using BdtShared.Models.App;

namespace BdtServer.AppServices.Weekday;

public interface IWeekdayService : IGenericApiService
{
    Task<ApiWrapper<IEnumerable<WeekdayDto>?>> GetAllWeekdays();
}
