using BdtApplication.AppServices.GenericApi;
using BdtDomain.Dtos.Levels;
using BdtDomain.Models.App;

namespace BdtApplication.AppServices.Levels;

public interface ILevelService : IGenericApiService
{
    Task<ApiWrapper<IEnumerable<LevelDto>?>> GetAllLevels();
}
