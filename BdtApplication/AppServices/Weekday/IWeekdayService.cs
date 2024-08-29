using BdtApplication.AppServices.GenericApi;
using BdtDomain.Dtos.WeekDay;
using BdtDomain.Models.App;

namespace BdtApplication.AppServices.Weekday;

public interface IWeekdayService : IGenericApiService
{
    Task<ApiWrapper<IEnumerable<WeekdayDto>?>> GetAllWeekdays();
}
