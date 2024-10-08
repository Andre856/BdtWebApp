using Bdt.Client.AppServices.App;
using Bdt.Client.AppServices.BdtProduct;
using Bdt.Client.AppServices.Checkout;
using Bdt.Client.AppServices.Claims;
using Bdt.Client.AppServices.Dialog;
using Bdt.Client.AppServices.GenericApi;
using Bdt.Client.AppServices.Levels;
using Bdt.Client.AppServices.Planner;
using Bdt.Client.AppServices.Session;
using Bdt.Client.AppServices.Stripe;
using Bdt.Client.AppServices.Theme;
using Bdt.Client.AppServices.Weekday;
using Bdt.Client.AppServices.Workouts;
using Bdt.Client.AppServices.WorkoutType;
using Bdt.Client.Handller;
using Bdt.Client.Provider;
using Bdt.Client.Provider.Token;
using Bdt.Client;
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
