using BdtClient.AppServices.GenericApi;
using Bdt.Shared.Dtos.WeekDay;
using Bdt.Shared.Models.App;
using Bdt.Shared.Static;
using Newtonsoft.Json;

namespace BdtClient.AppServices.Weekday;

public class WeekdayService : GenericApiService, IWeekdayService
{
    public WeekdayService(IHttpClientFactory httpClientFactory) : base(httpClientFactory) { }

    public async Task<ApiWrapper<IEnumerable<WeekdayDto>>> GetAllWeekdays()
    {
        try
        {
            if (StaticWeekdays.WeekdayContent is not null)
                return StaticWeekdays.WeekdayContent;

            var response = await _httpClient.GetAsync("v1/Weekday/GetAll");

            string contentString = await response.Content.ReadAsStringAsync();
            var content = JsonConvert.DeserializeObject<ApiWrapper<IEnumerable<WeekdayDto>>>(contentString);

            StaticWeekdays.WeekdayContent = content;
            return content;
        }
        catch (HttpRequestException ex)
        {
            return ApiWrapper<IEnumerable<WeekdayDto>>.Failed($"Request error. Exception: {ex.Message}");
        }
        catch (Exception ex)
        {
            return ApiWrapper<IEnumerable<WeekdayDto>>.Failed($"Api call failed. Exception: {ex.Message}");
        }
    }
}
