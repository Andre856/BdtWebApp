using BdtApi.Application.Services.BdtKeyVault;
using BdtApi.Application.Services.BdtProduct;
using BdtApi.Application.Services.Email;
using BdtApi.Application.Services.Generic;
using BdtApi.Application.Services.Level;
using BdtApi.Application.Services.Planner;
using BdtApi.Application.Services.Weekdays;
using BdtApi.Application.Services.Workout;
using BdtApi.Application.Services.WorkoutType;
using BdtApi.Application.Services.WorkoutValues;
using Microsoft.Extensions.DependencyInjection;

namespace BdtApi.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
       services.AddScoped(typeof(IGenericService<,,>), typeof(GenericService<,,>));
       services.AddScoped(typeof(IGenericService<,,,>), typeof(GenericService<,,,>));
       services.AddScoped(typeof(IGenericService<,,,,>), typeof(GenericService<,,,,>));
       services.AddScoped(typeof(IGenericService<,,,,,>), typeof(GenericService<,,,,,>));

       services.AddScoped<IEmailService, EmailService>();
       services.AddScoped<IWorkoutService, WorkoutService>();
       services.AddScoped<IPlannerService, PlannerService>();
       services.AddScoped<ILevelService, LevelService>();
       services.AddScoped<IWeekdayService, WeekdayService>();
       services.AddScoped<IWorkoutTypeService, WorkoutTypeService>();
       services.AddScoped<IBdtProductService, BdtProductService>();
       services.AddScoped<IBdtKeyVaultService, BdtKeyVaultService>();
       services.AddScoped<IWorkoutValuesService, WorkoutValuesService>();

        return services;
    }
}
