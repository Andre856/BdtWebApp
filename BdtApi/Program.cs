using Azure.Identity;
using BdtApi.Application;
using BdtApi.Domain.Helpers;
using BdtApi.Infrastructure;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

var kvUrl = Environment.GetEnvironmentVariable("AZURE_KEY_VAULT_URL") 
    ?? throw new Exception("Could not find key vault url.");

builder.Configuration.AddAzureKeyVault(new Uri(kvUrl), new DefaultAzureCredential());

AppEnvironmentVarsHelper.SetEnvironmentVars(
    builder.Configuration.GetSection("ProdDbConnectionString").Value,
    builder.Configuration.GetSection("DevDbConnectionString").Value,
    builder.Configuration.GetSection("TestDbConnectionString").Value);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();

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

app.UseCors("AllowBdtUser");

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
