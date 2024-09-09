using AutoMapper;
using Bdt.Api.Application.Services.Interfaces;
using Bdt.Api.Domain.Entities;
using Bdt.Api.Infrastructure.Repositories.Interfaces;
using Bdt.Shared.Dtos.WeekDay;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace Bdt.Api.Application.Services;

public class WeekdayService : GenericService<int, WeekdayEntity, WeekdayDto>, IWeekdayService
{
    private const string WeekdayCache = "WeekdayCache";
    private readonly IMemoryCache _cache;

    public WeekdayService(IReadRepository<int, WeekdayEntity> repository, IMemoryCache cache, IMapper mapper, ILogger<WeekdayService> logger)
        : base(repository, mapper, logger)
    {
        _cache = cache;
    }

    public override async Task<IEnumerable<WeekdayDto>> GetAllAsync()
    {
        try
        {
            if (!_cache.TryGetValue(WeekdayCache, out IEnumerable<WeekdayEntity>? weekdays))
            {

                weekdays = await _repository.GetAllAsync();
                _cache.Set(WeekdayCache, weekdays, TimeSpan.FromDays(1));
            }

            if (weekdays is null)
                return Enumerable.Empty<WeekdayDto>();

            return _mapper.Map<IEnumerable<WeekdayDto>>(weekdays);
        }
        catch (Exception ex)
        {
            throw BuildExceptionToThrow(ex);
        }
    }
}