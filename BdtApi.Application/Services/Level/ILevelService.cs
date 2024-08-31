using BdtApi.Application.Services.Generic;
using BdtApi.Domain.Entities;
using BdtShared.Dtos.Levels;

namespace BdtApi.Application.Services.Level;

public interface ILevelService : IGenericService<int, LevelEntity, LevelDto>
{

}
