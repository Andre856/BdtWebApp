using Azure.Identity;
using BdtApi.Context;
using BdtApi.CustomClaims;
using BdtApi.Helpers;
using BdtApi.Managers;
using BdtApi.Mapper;
using BdtApi.Repository;
using BDtApi.ApiServices.BdtKeyVault;
using BDtApi.ApiServices.BdtProduct;
using BDtApi.ApiServices.Email;
using BDtApi.ApiServices.Generic;
using BDtApi.ApiServices.Level;
using BDtApi.ApiServices.Planner;
using BDtApi.ApiServices.Weekdays;
using BDtApi.ApiServices.Workout;
using BDtApi.ApiServices.WorkoutType;
using BDtApi.ApiServices.WorkoutValues;
using BdtShared.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Stripe;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var kvUrl = Environment.GetEnvironmentVariable("AZURE_KEY_VAULT_URL") ?? throw new Exception("Could not find key vault url.");
builder.Configuration.AddAzureKeyVault(new Uri(kvUrl), new DefaultAzureCredential());

AppEnvironmentVarsHelper.SetEnvironmentVars(
    builder.Configuration.GetSection("ProdDbConnectionString").Value,
    builder.Configuration.GetSection("DevDbConnectionString").Value,
    builder.Configuration.GetSection("TestDbConnectionString").Value);

builder.Services.AddScoped(typeof(IGenericService<,,>), typeof(GenericService<,,>));
builder.Services.AddScoped(typeof(IGenericService<,,,>), typeof(GenericService<,,,>));
builder.Services.AddScoped(typeof(IGenericService<,,,,>), typeof(GenericService<,,,,>));
builder.Services.AddScoped(typeof(IGenericService<,,,,,>), typeof(GenericService<,,,,,>));
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IWorkoutService, WorkoutService>();
builder.Services.AddScoped<IPlannerService, PlannerService>();
builder.Services.AddScoped<ILevelService, LevelService>();
builder.Services.AddScoped<IWeekdayService, WeekdayService>();
builder.Services.AddScoped<IWorkoutTypeService, WorkoutTypeService>();
builder.Services.AddScoped<IBdtProductService, BdtProductService>();
builder.Services.AddScoped<IBdtKeyVaultService, BdtKeyVaultService>();
builder.Services.AddScoped<IWorkoutValuesService, WorkoutValuesService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMemoryCache();
builder.Services.AddScoped<StripeManager>();
builder.Services.AddScoped(typeof(IReadRepository<,>), typeof(ReadRepository<,>));
builder.Services.AddScoped(typeof(ICreateRepository<,>), typeof(CreateRepository<,>));
builder.Services.AddScoped(typeof(IUpdateRepository<,>), typeof(UpdateRepository<,>));
builder.Services.AddScoped(typeof(IDeleteRepository<,>), typeof(DeleteRepository<,>));

builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);

builder.Services.AddIdentity<UserEntity, IdentityRole>()
    .AddEntityFrameworkStores<BdtDbContext>()
    .AddClaimsPrincipalFactory<CustomUserClaimsPrincipalFactory>()
    .AddDefaultTokenProviders();

builder.Services.AddApiVersioning(x =>
{
    x.AssumeDefaultVersionWhenUnspecified = true;
    x.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
    x.ReportApiVersions = true;
});

builder.Services.AddVersionedApiExplorer(setup =>
{
    setup.GroupNameFormat = "'v'VVV";
    setup.SubstituteApiVersionInUrl = true;
});

builder.Services.AddAuthentication(opts => { opts.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme; }).AddJwtBearer(jwtOptions =>
{
    var key = Encoding.UTF8.GetBytes(builder.Configuration["JwtKey"]);
    jwtOptions.SaveToken = true;
    jwtOptions.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["JwtIssuer"],
        ValidAudience = builder.Configuration["JwtAudience"],
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddSwaggerGen(swagger =>
{
    swagger.SwaggerDoc("v1",
        new OpenApiInfo
        {
            Title = "BdtApi",
            Version = "v1",
            Description = "Busy Dad Training API"
        });

    var securitySchema = new OpenApiSecurityScheme
    {
        Description = "Authorization header using Bearer scheme. Example: 'Authorization: Bearer {token}'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = "Bearer"
        }
    };

    swagger.AddSecurityDefinition(securitySchema.Reference.Id, securitySchema);
    swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                { securitySchema, Array.Empty<string>() }
            });
});

var connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING")
        ?? throw new Exception("Could not find prod db connection string.");

builder.Services.AddDbContext<BdtDbContext>(options => options.UseSqlServer(connectionString));


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBdtUser",
        builder =>
        {
            builder.WithOrigins("https://localhost:7114")
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    var provider = app.Services.GetService<IApiVersionDescriptionProvider>();
    app.UseSwaggerUI(options =>
    {
        foreach (var description in provider.ApiVersionDescriptions)
        {
            options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName);
        }
    });
}
else
{
    app.UseSwagger();

    var provider = app.Services.GetService<IApiVersionDescriptionProvider>();
    app.UseSwaggerUI(options =>
    {
        foreach (var description in provider.ApiVersionDescriptions)
        {
            options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName);
        }
    });
}

app.UseCors("AllowBdtUser");

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
