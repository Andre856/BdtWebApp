using BdtServer.AppServices.GenericApi;
using BdtShared.Dtos.Levels;
using BdtShared.Models.App;

namespace BdtServer.AppServices.Levels;

public interface ILevelService : IGenericApiService
{
    Task<ApiWrapper<IEnumerable<LevelDto>?>> GetAllLevels();
}
