using AutoMapper;
using BdtApi.Domain.Entities;
using BdtShared.Dtos.BdtProduct;
using BdtShared.Dtos.Levels;
using BdtShared.Dtos.Planner;
using BdtShared.Dtos.Stripe;
using BdtShared.Dtos.Users;
using BdtShared.Dtos.WeekDay;
using BdtShared.Dtos.Workouts;
using BdtShared.Dtos.WorkoutType;
using BdtShared.Dtos.WorkoutValues;
using Microsoft.AspNetCore.Identity;

namespace BdtApi.Infrastructure.Mapper;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<SubscriptionsDto, SubscriptionsEntity>();
        CreateMap<SubscriptionsEntity, SubscriptionsDto>();

        CreateMap<StripeCustomersEntity, StripeCustomerDto>();
        CreateMap<StripeCustomerDto, StripeCustomersEntity>();

        CreateMap<BdtProductEntity, BdtProductDto>();
        CreateMap<BdtProductDto, BdtProductEntity>();

        CreateMap<RegisterUserDto, UserEntity>();
        CreateMap<UserRoleDto, IdentityRole>();

        CreateMap<WorkoutEntity, WorkoutDto>();
        CreateMap<WorkoutDto, WorkoutEntity>();
        CreateMap<UpdateWorkoutDto, WorkoutDto>();
        CreateMap<CreateWorkoutDto, WorkoutDto>();
        CreateMap<CreateWorkoutDto, WorkoutEntity>();
        CreateMap<DeleteWorkoutDto, WorkoutDto>();
        CreateMap<WorkoutDto, DeleteWorkoutDto>();

        CreateMap<WorkoutValuesEntity, WorkoutValuesDto>();
        CreateMap<WorkoutValuesDto, WorkoutValuesEntity>();
        CreateMap<WorkoutValuesEntity, CreateWorkoutValuesDto>();
        CreateMap<CreateWorkoutValuesDto, WorkoutValuesEntity>();
        CreateMap<WorkoutValuesDto, DeleteWorkoutValuesDto>();

        CreateMap<PlannerEntity, PlannerDto>()
            .ForMember(dest => dest.WeekDay, opt => opt.MapFrom(src => src.Weekdays))
            .ForMember(dest => dest.WorkoutType, opt => opt.MapFrom(src => src.WorkoutType));
        CreateMap<PlannerDto, PlannerEntity>();
        CreateMap<UpdatePlannerDto, PlannerDto>();
        CreateMap<CreatePlannerDto, PlannerDto>();
        CreateMap<CreatePlannerDto, PlannerEntity>();
        CreateMap<PlannerDto, DeletePlannerDto>();
        CreateMap<PlannerDto, CreatePlannerDto>();

        CreateMap<WeekdayEntity, WeekdayDto>();
        CreateMap<WeekdayDto, WeekdayEntity>();

        CreateMap<WorkoutTypeEntity, WorkoutTypeDto>();
        CreateMap<WorkoutTypeDto, WorkoutTypeEntity>();

        CreateMap<LevelEntity, LevelDto>();
        CreateMap<LevelDto, LevelEntity>();
    }
}
