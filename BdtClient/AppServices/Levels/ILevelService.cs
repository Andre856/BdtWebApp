using BdtClient.AppServices.GenericApi;
using Bdt.Shared.Dtos.Levels;
using Bdt.Shared.Models.App;

namespace BdtClient.AppServices.Levels;

public interface ILevelService : IGenericApiService
{
    Task<ApiWrapper<IEnumerable<LevelDto>?>> GetAllLevels();
}
