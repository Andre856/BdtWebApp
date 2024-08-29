using BdtClient.AppServices.GenericApi;
using BdtShared.Dtos.Levels;
using BdtShared.Models.App;

namespace BdtClient.AppServices.Levels;

public interface ILevelService : IGenericApiService
{
    Task<ApiWrapper<IEnumerable<LevelDto>?>> GetAllLevels();
}
