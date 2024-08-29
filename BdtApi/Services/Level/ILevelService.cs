using BDtApi.ApiServices.Generic;
using BdtShared.Dtos.Levels;
using BdtShared.Entities;

namespace BDtApi.ApiServices.Level;

public interface ILevelService : IGenericService<int, LevelEntity, LevelDto>
{

}
