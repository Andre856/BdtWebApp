using AutoMapper;
using BdtApi.Repository;
using BDtApi.ApiServices.Generic;
using BdtShared.Dtos.WeekDay;
using BdtShared.Entities;
using Microsoft.Extensions.Caching.Memory;

namespace BDtApi.ApiServices.Weekdays;

public class WeekdayService : GenericService<int, WeekdayEntity, WeekdayDto>, IWeekdayService
{
    private const string WeekdayCache = "WeekdayCache";
    private readonly IMemoryCache _cache;

    public WeekdayService(IReadRepository<int, WeekdayEntity> repository, IMemoryCache cache, IMapper mapper)
        : base(repository, mapper)
    {
        _cache = cache;
    }

    public override async Task<IEnumerable<WeekdayDto>> GetAllAsync()
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
}