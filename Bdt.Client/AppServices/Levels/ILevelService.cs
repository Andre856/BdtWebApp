using Bdt.Shared.Dtos.Levels;
using Bdt.Shared.Models.App;
using Bdt.Client.AppServices.GenericApi;

namespace Bdt.Client.AppServices.Levels;

public interface ILevelService : IGenericApiService
{
    Task<ApiWrapper<IEnumerable<LevelDto>?>> GetAllLevels();
}
