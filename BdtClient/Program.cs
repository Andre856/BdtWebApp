using BdtClient;
using BdtClient.AppServices.App;
using BdtClient.AppServices.BdtProduct;
using BdtClient.AppServices.Checkout;
using BdtClient.AppServices.Claims;
using BdtClient.AppServices.Dialog;
using BdtClient.AppServices.GenericApi;
using BdtClient.AppServices.Levels;
using BdtClient.AppServices.Planner;
using BdtClient.AppServices.Session;
using BdtClient.AppServices.Stripe;
using BdtClient.AppServices.Theme;
using BdtClient.AppServices.Weekday;
using BdtClient.AppServices.Workouts;
using BdtClient.AppServices.WorkoutType;
using BdtClient.Handller;
using BdtClient.Provider;
using BdtClient.Provider.Token;
using Blazored.Toast;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddMudServices();
builder.Services.AddBlazoredToast();

var uri = new Uri("https://bdtapi-ghgqdkfbdxb2etgb.eastus-01.azurewebsites.net/api/");
//var uri = new Uri("https://localhost:7052/api/");

builder.Services.AddHttpClient("NoAuthBdtApi", client => { client.BaseAddress = uri; });
builder.Services.AddHttpClient("BdtApi", client => { client.BaseAddress = uri; })
    .AddHttpMessageHandler(sp => { return sp.GetRequiredService<CustomAuthorisationMessageHandler>(); });

builder.Services.AddAuthorizationCore();

builder.Services.AddSingleton<AuthorizationMessageHandler>();
builder.Services.AddSingleton<CustomAuthorisationMessageHandler>();
builder.Services.AddSingleton<CustomAuthStateProvider>();
builder.Services.AddSingleton<AuthenticationStateProvider>(sp => sp.GetRequiredService<CustomAuthStateProvider>());
builder.Services.AddSingleton<IAppService, AppService>();
builder.Services.AddSingleton<ITokenProvider, TokenProvider>();
builder.Services.AddSingleton<IClaimService, ClaimService>();
builder.Services.AddSingleton<BdtThemeService>();

builder.Services.AddScoped<IGenericApiService, GenericApiService>();
builder.Services.AddScoped<IWorkoutService, WorkoutService>();
builder.Services.AddScoped<IPlannerService, PlannerService>();
builder.Services.AddScoped<IWeekdayService, WeekdayService>();
builder.Services.AddScoped<IWorkoutTypeService, WorkoutTypeService>();
builder.Services.AddScoped<ILevelService, LevelService>();
builder.Services.AddScoped<IBdtProductService, BdtProductService>();
builder.Services.AddScoped<ICheckoutService, CheckoutService>();
builder.Services.AddScoped<IStripeService, StripeService>();
builder.Services.AddScoped<ISessionService, SessionService>();
builder.Services.AddScoped<IBdtDialogService, BdtDialogService>();

await builder.Build().RunAsync();
