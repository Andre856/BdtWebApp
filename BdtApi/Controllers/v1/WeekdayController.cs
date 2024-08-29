using AutoMapper;
using BDtApi.ApiServices.Weekdays;
using BdtShared.Dtos.WeekDay;
using BdtShared.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BdtApi.Controllers.v1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class WeekdayController : BaseController<int, WeekdayEntity, WeekdayDto>
    {
        public WeekdayController(IMapper mapper, IWeekdayService service)
            : base(mapper, service)
        { }
    }
}
