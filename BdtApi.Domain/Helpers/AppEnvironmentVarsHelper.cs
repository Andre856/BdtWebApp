using BdtShared.Enums;

namespace BdtApi.Domain.Helpers;
public class AppEnvironmentVarsHelper
{
    public static void SetGlobalVariables()
    {
        string? environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

        switch (environment)
        {
            case null:
                Globals.Environment = EnvironmentEnums.Production;
                break;
            case "Development":
                Globals.Environment = EnvironmentEnums.Devevelopment;
                break;
            case "Testing":
                Globals.Environment = EnvironmentEnums.Testing;
                break;
            case "Staging":
                Globals.Environment = EnvironmentEnums.Staging;
                break;
        }
    }

    public static void SetEnvironmentVars(string? prodConnectionString, string? devConnectionString, string? testConnectionString)
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
