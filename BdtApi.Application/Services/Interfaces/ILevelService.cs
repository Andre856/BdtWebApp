using BdtApi.Domain.Entities;
using BdtShared.Dtos.Levels;

namespace BdtApi.Application.Services.Interfaces;

public interface ILevelService : IGenericService<int, LevelEntity, LevelDto>
{

}
