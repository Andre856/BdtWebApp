using BdtApi.Application.Services;
using BdtApi.Application.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace BdtApi.Application;

public static class Application
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
