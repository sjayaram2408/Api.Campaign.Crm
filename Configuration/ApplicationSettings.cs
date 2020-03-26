
namespace Api.Campaign.Crm
{
    public class ApplicationSettings
    {
        public CorsSettings Cors { get; set; }
        public AuthenticationSettings Authentication { get; set; }
        public SwaggerSettings Swagger { get; set; }
        public LoggingSettings Logging { get; set; }
    }
}