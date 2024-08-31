using BdtApi.Application.Services.Generic;
using BdtApi.Domain.Entities;
using BdtShared.Dtos.WeekDay;

namespace BdtApi.Application.Services.Weekdays;

public interface IWeekdayService : IGenericService<int, WeekdayEntity, WeekdayDto> { }
