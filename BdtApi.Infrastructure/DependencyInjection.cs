using BdtApi.Domain.Entities;
using BdtApi.Infrastructure.Context;
using BdtApi.Infrastructure.CustomClaims;
using BdtApi.Infrastructure.Managers;
using BdtApi.Infrastructure.Mapper;
using BdtApi.Infrastructure.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace BdtApi.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfigurationManager config)
    {
        services.AddMemoryCache();
        services.AddAutoMapper(typeof(MappingProfile).Assembly);

        services.AddScoped<StripeManager>();

        services.AddScoped(typeof(IReadRepository<,>), typeof(ReadRepository<,>));
        services.AddScoped(typeof(ICreateRepository<,>), typeof(CreateRepository<,>));
        services.AddScoped(typeof(IUpdateRepository<,>), typeof(UpdateRepository<,>));
        services.AddScoped(typeof(IDeleteRepository<,>), typeof(DeleteRepository<,>));

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

        services.AddDbContext<BdtDbContext>(options => options.UseSqlServer(connectionString));

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
}
