using Bdt.Client.AppServices.GenericApi;
using Bdt.Shared.Dtos.Planner;
using Bdt.Shared.Models.App;
using Newtonsoft.Json;
using System.Net.Http.Json;

namespace Bdt.Client.AppServices.Planner;

public class PlannerService : GenericApiService, IPlannerService
{
    public PlannerService(IHttpClientFactory httpClientFactory) : base(httpClientFactory) { }

    public async Task<ApiWrapper<IEnumerable<PlannerDto>?>> GetUserPlans()
    {
        try
        {
            var response = await _httpClient.GetAsync("v1/Planner/GetUserPlans");

            string contentString = await response.Content.ReadAsStringAsync();
            var content = JsonConvert.DeserializeObject<ApiWrapper<IEnumerable<PlannerDto>?>>(contentString);

            return content;
        }
        catch (HttpRequestException ex)
        {
            return ApiWrapper<IEnumerable<PlannerDto>>.Failed($"Request error. Exception: {ex.Message}");
        }
        catch (Exception ex)
        {
            return ApiWrapper<IEnumerable<PlannerDto>>.Failed($"Api call failed. Exception: {ex.Message}");
        }
    }

    public async Task<ApiWrapper<PlannerDto>> CreateUserPlans(CreatePlannerDto createPlans)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("v1/Planner/Create", createPlans);

            string contentString = await response.Content.ReadAsStringAsync();
            var content = JsonConvert.DeserializeObject<ApiWrapper<PlannerDto>>(contentString);

            return content;
        }
        catch (HttpRequestException ex)
        {
            return ApiWrapper<PlannerDto>.Failed($"Request error. Exception: {ex.Message}");
        }
        catch (Exception ex)
        {
            return ApiWrapper<PlannerDto>.Failed($"Api call failed. Exception: {ex.Message}");
        }
    }

    public async Task<ApiWrapper<PlannerDto>> DeleteUserPlans(Guid id)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"v1/Planner/Delete/{id}");

            string contentString = await response.Content.ReadAsStringAsync();
            var content = JsonConvert.DeserializeObject<ApiWrapper<PlannerDto>>(contentString);

            return content;
        }
        catch (HttpRequestException ex)
        {
            return ApiWrapper<PlannerDto>.Failed($"Request error. Exception: {ex.Message}");
        }
        catch (Exception ex)
        {
            return ApiWrapper<PlannerDto>.Failed($"Api call failed. Exception: {ex.Message}");
        }
    }

    public async Task<ApiWrapper<PlannerDto>> UpdateUserPlans(UpdatePlannerDto updatePlans)
    {
        try
        {
            var response = await _httpClient.PatchAsJsonAsync("v1/Planner/Update", updatePlans);

            string contentString = await response.Content.ReadAsStringAsync();
            var content = JsonConvert.DeserializeObject<ApiWrapper<PlannerDto>>(contentString);

            return content;
        }
        catch (HttpRequestException ex)
        {
            return ApiWrapper<PlannerDto>.Failed($"Request error. Exception: {ex.Message}");
        }
        catch (Exception ex)
        {
            return ApiWrapper<PlannerDto>.Failed($"Api call failed. Exception: {ex.Message}");
        }
    }
}
