using Bdt.Api.Infrastructure.Context;
using Bdt.Api.Infrastructure.CustomClaims;
using Bdt.Api.Infrastructure.Managers;
using Bdt.Api.Infrastructure.Mapper;
using Bdt.Api.Infrastructure.Repositories;
using Bdt.Api.Infrastructure.Repositories.Interfaces;
using Bdt.Api.Domain.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using Bdt.Api.Infrastructure.Interceptors;

namespace Bdt.Api.Infrastructure;

public static class Infrastructure
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfigurationManager config)
    {
        services.AddMemoryCache();
        services.AddAutoMapper(typeof(MappingProfile).Assembly);

        services.AddScoped<AuditInterceptor>();
        services.AddScoped<StripeManager>();

        services.AddScoped(typeof(IReadRepository<,>), typeof(ReadRepository<,>));
        services.AddScoped(typeof(ICreateRepository<,>), typeof(CreateRepository<,>));
        services.AddScoped(typeof(IUpdateRepository<,>), typeof(UpdateRepository<,>));
        services.AddScoped(typeof(IDeleteRepository<,>), typeof(DeleteRepository<,>));

        services.AddSwaggerGen(swagger =>
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

        services.AddIdentity<UserEntity, IdentityRole>()
            .AddEntityFrameworkStores<BdtDbContext>()
            .AddClaimsPrincipalFactory<CustomUserClaimsPrincipalFactory>()
            .AddDefaultTokenProviders();

        services.AddApiVersioning(x =>
        {
            x.AssumeDefaultVersionWhenUnspecified = true;
            x.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
            x.ReportApiVersions = true;
        });

        services.AddVersionedApiExplorer(setup =>
        {
            setup.GroupNameFormat = "'v'VVV";
            setup.SubstituteApiVersionInUrl = true;
        });

        services.AddAuthentication(opts => { opts.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme; }).AddJwtBearer(jwtOptions =>
        {
            var key = Encoding.UTF8.GetBytes(config["JwtKey"]);
            jwtOptions.SaveToken = true;
            jwtOptions.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = config["JwtIssuer"],
                ValidAudience = config["JwtAudience"],
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ClockSkew = TimeSpan.Zero
            };
        });

        var connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING")
        ?? throw new Exception("Could not find prod db connection string.");

        services.AddDbContext<BdtDbContext>((serviceProvider, options) =>
        {
            options.UseSqlServer(connectionString);
            options.AddInterceptors(serviceProvider.GetRequiredService<AuditInterceptor>());
        });

        services.AddCors(options =>
        {
            options.AddPolicy("AllowBdtUser",
                builder =>
                {
                    builder.WithOrigins("https://localhost:7114", "https://purple-river-0804f4b0f.5.azurestaticapps.net")
                           .AllowAnyHeader()
                           .AllowAnyMethod();
                });
        });

        return services;
    }

    public static void SetEnvironmentVariables(this IConfiguration config)
    {
        SetDatabaseStrings(config.GetSection("ProdDbConnectionString").Value,
            config.GetSection("DevDbConnectionString").Value,
            config.GetSection("TestDbConnectionString").Value);
    }

    private static void SetDatabaseStrings(string? prodConnectionString, string? devConnectionString, string? testConnectionString)
    {
        string? connectionString = null;
        string? environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

        switch (environment)
        {
            case null:
                connectionString = prodConnectionString;
                break;
            case "Development":
                connectionString = devConnectionString;
                break;
            case "Testing":
                connectionString = testConnectionString;
                break;
            case "Staging":
                connectionString = prodConnectionString;
                break;
        }

        if (connectionString is null)
            throw new Exception("Could not find connection string.");

        Environment.SetEnvironmentVariable("CONNECTION_STRING", connectionString);
    }
}
