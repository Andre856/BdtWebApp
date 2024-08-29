using AutoMapper;
using BDtApi.ApiServices.WorkoutType;
using BdtShared.Dtos.WorkoutType;
using BdtShared.Entities;
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
