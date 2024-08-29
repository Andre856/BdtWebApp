using BdtDomain.Entities;
using BdtApplication.ApiServices.Generic;
using BdtDomain.Dtos.Levels;

namespace BdtApplication.ApiServices.Level;

public interface ILevelServiceApi : IGenericService<int, LevelEntity, LevelDto>
{

}
