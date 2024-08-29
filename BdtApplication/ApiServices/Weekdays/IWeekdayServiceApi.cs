using BdtApplication.ApiServices.Generic;
using BdtDomain.Dtos.WeekDay;
using BdtDomain.Entities;

namespace BdtApplication.ApiServices.Weekdays;

public interface IWeekdayServiceApi : IGenericService<int, WeekdayEntity, WeekdayDto> { }
