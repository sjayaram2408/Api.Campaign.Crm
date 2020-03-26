using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using Serilog;
using Serilog.Formatting.Json;
using Serilog.Sinks.SystemConsole.Themes;

namespace Api.Campaign.Crm
{
    public static class LoggerConfigurationExtensions
    {

        public static void ConfigureLocalLogger(this IConfiguration configuration)
        {
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .Destructure.AsScalar<JObject>()
                .Destructure.AsScalar<JArray>()
                .Enrich.FromLogContext()
                .WriteTo.Console(theme: AnsiConsoleTheme.Code)
                .CreateLogger();
        }
    }
}
