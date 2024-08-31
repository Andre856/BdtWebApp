using BdtApi.Domain.Entities;
using BdtShared.Dtos.WeekDay;

namespace BdtApi.Application.Services.Interfaces;

public interface IWeekdayService : IGenericService<int, WeekdayEntity, WeekdayDto> { }
