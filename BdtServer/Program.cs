using BdtServer.AppServices.App;
using BdtServer.AppServices.BdtProduct;
using BdtServer.AppServices.Checkout;
using BdtServer.AppServices.GenericApi;
using BdtServer.AppServices.Levels;
using BdtServer.AppServices.Planner;
using BdtServer.AppServices.Session;
using BdtServer.AppServices.Stripe;
using BdtServer.AppServices.Theme;
using BdtServer.AppServices.User;
using BdtServer.AppServices.Weekday;
using BdtServer.AppServices.Workouts;
using BdtServer.AppServices.WorkoutType;
using BdtServer.Handller;
using Microsoft.AspNetCore.Components.Server.Circuits;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.#
builder.Services.AddHttpContextAccessor();
builder.Services.AddMudServices();

builder.Services.AddHttpClient("BdtApi", client =>
{
    client.BaseAddress = new Uri("https://bdtapi-ghgqdkfbdxb2etgb.eastus-01.azurewebsites.net/api/");
});

builder.Services.AddScoped<IGenericApiService, GenericApiService>();
builder.Services.AddScoped<IAppService, AppService>();
builder.Services.AddScoped<IWorkoutService, WorkoutService>();
builder.Services.AddScoped<IPlannerService, PlannerService>();
builder.Services.AddScoped<IWeekdayService, WeekdayService>();
builder.Services.AddScoped<IWorkoutTypeService, WorkoutTypeService>();
builder.Services.AddScoped<ILevelService, LevelService>();
builder.Services.AddScoped<IBdtProductService, BdtProductService>();
builder.Services.AddScoped<ICheckoutService, CheckoutService>();
builder.Services.AddScoped<IStripeService, StripeService>();
builder.Services.AddScoped<ISessionService, SessionService>();
builder.Services.AddScoped<IUserService, UserService>();

//services.AddScoped<CustomAuthenticationStateProvider>();
builder.Services.AddScoped<ThemeService>();

builder.Services.TryAddEnumerable(ServiceDescriptor.Scoped<CircuitHandler, UserCircuitHandler>());

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
