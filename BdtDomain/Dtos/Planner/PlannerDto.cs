﻿using BdtShared.Dtos;
using BdtShared.Dtos.WeekDay;
using BdtShared.Dtos.WorkoutType;

namespace BdtShared.Dtos.Planner;

public class PlannerDto : IBaseDto<Guid>, IUserDto
{
    public Guid Id { get; set; }
    public string UserId { get; set; }
    public int WeekDayId { get; set; }
    public int WorkoutTypeId { get; set; }
    public decimal WorkoutDuration { get; set; }
    public WeekdayDto WeekDay { get; set; }
    public WorkoutTypeDto WorkoutType { get; set; }
}
