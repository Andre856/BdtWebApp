using AutoMapper;
using BdtApi.Application.Services.WorkoutType;
using BdtApi.Domain.Entities;
using BdtShared.Dtos.WorkoutType;
using Microsoft.AspNetCore.Mvc;

namespace BdtApi.Controllers.v1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class WorkoutTypeController : BaseController<int, WorkoutTypeEntity, WorkoutTypeDto>
{
    public WorkoutTypeController(IMapper mapper, IWorkoutTypeService service)
        : base(mapper, service)
    { }
}
