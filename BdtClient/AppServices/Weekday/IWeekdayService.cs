using BdtClient.AppServices.GenericApi;
using Bdt.Shared.Dtos.WeekDay;
using Bdt.Shared.Models.App;

namespace BdtClient.AppServices.Weekday;

public interface IWeekdayService : IGenericApiService
{
    Task<ApiWrapper<IEnumerable<WeekdayDto>?>> GetAllWeekdays();
}
