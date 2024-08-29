using AutoMapper;
using BdtDomain.Entities;
using BdtDomain.Dtos.BdtProduct;
using BdtDomain.Dtos.Levels;
using BdtDomain.Dtos.Planner;
using BdtDomain.Dtos.Stripe;
using BdtDomain.Dtos.Users;
using BdtDomain.Dtos.WeekDay;
using BdtDomain.Dtos.Workouts;
using BdtDomain.Dtos.WorkoutType;
using BdtDomain.Dtos.WorkoutValues;
using Microsoft.AspNetCore.Identity;

namespace BdtInfrastructure.Mapper;

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
