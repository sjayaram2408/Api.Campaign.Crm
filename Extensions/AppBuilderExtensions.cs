using Api.Campaign.Crm.Configuration;
using Api.Campaign.Crm.Middleware;
using Microsoft.AspNetCore.Builder;
using System;
using System.Linq;

namespace Api.Campaign.Crm.Extensions
{
    public static class AppBuilderExtensions
    {
        public static void UseDefaultCorsPolicy(this IApplicationBuilder app, CorsSettings settings)
        {
            if (settings.AllowedOrigins == null)
            {
                throw new ArgumentNullException(nameof(settings.AllowedOrigins));
            }

            app.UseCors(builder => builder.WithOrigins(settings.AllowedOrigins.ToArray())
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials());
        }

        public static void UseRequestLogging(this IApplicationBuilder app)
        {
            app.UseMiddleware<RequestLoggingMiddleware>();
        }

        public static void UseRequestLogging(this IApplicationBuilder app, LoggingSettings settings)
        {
            app.UseMiddleware<RequestLoggingMiddleware>(settings);
        }
    }
}
