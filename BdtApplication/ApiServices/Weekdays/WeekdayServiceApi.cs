﻿using AutoMapper;
using BdtApplication.ApiServices.Generic;
using BdtDomain.Dtos.WeekDay;
using BdtDomain.Entities;
using BdtDomain.Repository;
using Microsoft.Extensions.Caching.Memory;

namespace BdtApplication.ApiServices.Weekdays;

public class WeekdayServiceApi : GenericService<int, WeekdayEntity, WeekdayDto>, IWeekdayServiceApi
{
    private const string WeekdayCache = "WeekdayCache";
    private readonly IMemoryCache _cache;

    public WeekdayServiceApi(IReadRepository<int, WeekdayEntity> repository, IMemoryCache cache, IMapper mapper)
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