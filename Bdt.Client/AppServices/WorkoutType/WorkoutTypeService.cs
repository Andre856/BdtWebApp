using Bdt.Shared.Dtos.WorkoutType;
using Bdt.Shared.Models.App;
using Bdt.Shared.Static;
using Newtonsoft.Json;
using Bdt.Client.AppServices.GenericApi;

namespace Bdt.Client.AppServices.WorkoutType;

public class WorkoutTypeService : GenericApiService, IWorkoutTypeService
{
    public WorkoutTypeService(IHttpClientFactory httpClientFactory) : base(httpClientFactory) { }

    public async Task<ApiWrapper<IEnumerable<WorkoutTypeDto>>> GetAllWorkoutTypes()
    {
        try
        {
            if (StaticWorkoutTypes.WorkoutTypeContent is not null)
                return StaticWorkoutTypes.WorkoutTypeContent;

            var response = await _httpClient.GetAsync("v1/WorkoutType/GetAll");

            string contentString = await response.Content.ReadAsStringAsync();
            var content = JsonConvert.DeserializeObject<ApiWrapper<IEnumerable<WorkoutTypeDto>>>(contentString);

            StaticWorkoutTypes.WorkoutTypeContent = content;
            return content;
        }
        catch (HttpRequestException ex)
        {
            return ApiWrapper<IEnumerable<WorkoutTypeDto>>.Failed($"Request error. Exception: {ex.Message}");
        }
        catch (Exception ex)
        {
            return ApiWrapper<IEnumerable<WorkoutTypeDto>>.Failed($"Api call failed. Exception: {ex.Message}");
        }
    }
}
