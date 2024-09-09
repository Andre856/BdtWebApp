using AutoMapper;
using Bdt.Api.Application.Services.Interfaces;
using Bdt.Api.Domain.Entities;
using Bdt.Shared.Dtos.WorkoutType;
using Microsoft.AspNetCore.Mvc;

namespace Bdt.Api.Controllers.v1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class WorkoutTypeController : BaseController<int, WorkoutTypeEntity, WorkoutTypeDto>
{
    public WorkoutTypeController(IMapper mapper, IWorkoutTypeService service)
        : base(mapper, service)
    { }
}
