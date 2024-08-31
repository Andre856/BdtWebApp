using BdtClient.AppServices.GenericApi;
using Bdt.Shared.Dtos.Workouts;
using Bdt.Shared.Models.App;
using Newtonsoft.Json;
using System.Text;

namespace BdtClient.AppServices.Workouts;

public class WorkoutService : GenericApiService, IWorkoutService
{
    public WorkoutService(IHttpClientFactory httpClientFactory) : base(httpClientFactory) { }

    public async Task<ApiWrapper<IEnumerable<WorkoutDto>>> GetUserWorkoutsLastMonth()
    {
        try
        {
            var response = await _httpClient.GetAsync("v1/Workout/GetUserWorkoutsLastMonth");

            string contentString = await response.Content.ReadAsStringAsync();
            var content = JsonConvert.DeserializeObject<ApiWrapper<IEnumerable<WorkoutDto>>>(contentString);

            return content;
        }
        catch (HttpRequestException ex)
        {
            return ApiWrapper<IEnumerable<WorkoutDto>>.Failed($"Request error. Exception: {ex.Message}");
        }
        catch (Exception ex)
        {
            return ApiWrapper<IEnumerable<WorkoutDto>>.Failed($"Api call failed. Exception: {ex.Message}");
        }
    }

    public async Task<ApiWrapper<IEnumerable<WorkoutDto>>> GetUserWorkouts()
    {
        try
        {
            var response = await _httpClient.GetAsync("v1/Workout/GetUserWorkouts");

            string contentString = await response.Content.ReadAsStringAsync();
            var content = JsonConvert.DeserializeObject<ApiWrapper<IEnumerable<WorkoutDto>>>(contentString);

            return content;
        }
        catch (HttpRequestException ex)
        {
            return ApiWrapper<IEnumerable<WorkoutDto>>.Failed($"Request error. Exception: {ex.Message}");
        }
        catch (Exception ex)
        {
            return ApiWrapper<IEnumerable<WorkoutDto>>.Failed($"Api call failed. Exception: {ex.Message}");
        }
    }

    public async Task<ApiWrapper<WorkoutDto>> CreateUserWorkout(CreateWorkoutDto createWorkout)
    {
        var serialisedString = JsonConvert.SerializeObject(createWorkout);

        try
        {
            var response = await _httpClient.PostAsync("v1/Workout/Create", new StringContent(serialisedString, Encoding.UTF8, "application/json"));

            string contentString = await response.Content.ReadAsStringAsync();
            var content = JsonConvert.DeserializeObject<ApiWrapper<WorkoutDto>>(contentString);

            return content;
        }
        catch (HttpRequestException ex)
        {
            return ApiWrapper<WorkoutDto>.Failed($"Request error. Exception: {ex.Message}");
        }
        catch (Exception ex)
        {
            return ApiWrapper<WorkoutDto>.Failed($"Api call failed. Exception: {ex.Message}");
        }
    }

    public async Task<ApiWrapper<WorkoutDto>> DeleteUserWorkout(Guid id)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"v1/Workout/Delete/{id}");

            string contentString = await response.Content.ReadAsStringAsync();
            var content = JsonConvert.DeserializeObject<ApiWrapper<WorkoutDto>>(contentString);

            return content;
        }
        catch (HttpRequestException ex)
        {
            return ApiWrapper<WorkoutDto>.Failed($"Request error. Exception: {ex.Message}");
        }
        catch (Exception ex)
        {
            return ApiWrapper<WorkoutDto>.Failed($"Api call failed. Exception: {ex.Message}");
        }
    }
}
