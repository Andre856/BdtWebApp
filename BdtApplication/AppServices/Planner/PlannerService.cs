using BdtDomain.Models.App;
using BdtApplication.AppServices.GenericApi;
using BdtDomain.Dtos.Planner;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace BdtApplication.AppServices.Planner;

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
        var serialisedString = JsonConvert.SerializeObject(createPlans);

        try
        {
            var response = await _httpClient.PostAsync("v1/Planner/Create", new StringContent(serialisedString, Encoding.UTF8, "application/json"));

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
        var serialisedString = JsonConvert.SerializeObject(updatePlans);

        try
        {
            var response = await _httpClient.PatchAsync("v1/Planner/Update", new StringContent(serialisedString, Encoding.UTF8, "application/json"));

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
