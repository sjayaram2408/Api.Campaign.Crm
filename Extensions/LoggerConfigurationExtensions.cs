using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using Serilog;
using Serilog.Formatting.Json;
using Serilog.Sinks.Elasticsearch;
using Serilog.Sinks.SystemConsole.Themes;

namespace Api.Campaign.Crm.Extensions
{
    public static class LoggerConfigurationExtensions
    {

        public static void ConfigureLocalLogger(this IConfiguration configuration)
        {
            var elasticUri = configuration["ElasticConfiguration:Uri"];

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .Destructure.AsScalar<JObject>()
                .Destructure.AsScalar<JArray>()
                .Enrich.FromLogContext()
                .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new System.Uri(elasticUri))
                {
                    AutoRegisterTemplate = true,
                })
                .WriteTo.Console(theme: AnsiConsoleTheme.Code)
                .CreateLogger();
        }
    }
}
