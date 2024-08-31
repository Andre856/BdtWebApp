using AutoMapper;
using BdtApi.Application.Services.Level;
using BdtApi.Domain.Entities;
using BdtShared.Dtos.Levels;
using Microsoft.AspNetCore.Mvc;

namespace BdtApi.Controllers.v1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class LevelController : BaseController<int, LevelEntity, LevelDto>
{
    public LevelController(IMapper mapper, ILevelService service)
        : base(mapper, service)
    { }
}
