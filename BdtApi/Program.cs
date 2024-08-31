using Azure.Identity;
using BdtApi.Application;
using BdtApi.Infrastructure;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

var kvUrl = Environment.GetEnvironmentVariable("AZURE_KEY_VAULT_URL") 
    ?? throw new Exception("Could not find key vault url.");

builder.Configuration.AddAzureKeyVault(new Uri(kvUrl), new DefaultAzureCredential());
builder.Configuration.SetEnvironmentVariables();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer()

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();

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
