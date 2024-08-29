using AutoMapper;
using BDtApi.ApiServices.Level;
using BdtShared.Dtos.Levels;
using BdtShared.Entities;
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
