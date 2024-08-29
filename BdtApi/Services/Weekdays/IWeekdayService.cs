using BDtApi.ApiServices.Generic;
using BdtShared.Dtos.WeekDay;
using BdtShared.Entities;

namespace BDtApi.ApiServices.Weekdays;

public interface IWeekdayService : IGenericService<int, WeekdayEntity, WeekdayDto> { }
