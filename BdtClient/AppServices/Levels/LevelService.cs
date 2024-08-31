using BdtClient.AppServices.GenericApi;
using Bdt.Shared.Dtos.Levels;
using Bdt.Shared.Models.App;
using Bdt.Shared.Static;
using Newtonsoft.Json;

namespace BdtClient.AppServices.Levels;

public class LevelService : GenericApiService, ILevelService
{
    public LevelService(IHttpClientFactory httpClientFactory) : base(httpClientFactory) { }

    public async Task<ApiWrapper<IEnumerable<LevelDto>>> GetAllLevels()
    {
        try
        {
            if (StaticLevels.LevelContent is not null)
                return StaticLevels.LevelContent;

            var response = await _httpClient.GetAsync("v1/Level/GetAll");

            string contentString = await response.Content.ReadAsStringAsync();
            var content = JsonConvert.DeserializeObject<ApiWrapper<IEnumerable<LevelDto>>>(contentString);

            StaticLevels.LevelContent = content;
            return content;
        }
        catch (HttpRequestException ex)
        {
            return ApiWrapper<IEnumerable<LevelDto>>.Failed($"Request error. Exception: {ex.Message}");
        }
        catch (Exception ex)
        {
            return ApiWrapper<IEnumerable<LevelDto>>.Failed($"Api call failed. Exception: {ex.Message}");
        }
    }
}
