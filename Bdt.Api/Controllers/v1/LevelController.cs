using AutoMapper;
using Bdt.Api.Application.Services.Interfaces;
using Bdt.Api.Controllers;
using Bdt.Api.Domain.Entities;
using Bdt.Shared.Dtos.Levels;
using Microsoft.AspNetCore.Mvc;

namespace Bdt.Api.Controllers.v1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class LevelController : BaseController<int, LevelEntity, LevelDto>
{
    public LevelController(IMapper mapper, ILevelService service)
        : base(mapper, service)
    { }
}
