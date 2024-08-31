using Bdt.Api.Domain.Entities;
using Bdt.Shared.Dtos.WeekDay;

namespace Bdt.Api.Application.Services.Interfaces;

public interface IWeekdayService : IGenericService<int, WeekdayEntity, WeekdayDto> { }
