using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Campaign.Crm.Configuration;
using Api.Campaign.Crm.Extensions;
using Api.Campaign.Crm.Filters;
using DealerSocket.Crm.Integrations.FacebookAudience.Interfaces;
using DealerSocket.Crm.Integrations.FacebookAudience.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Serilog;

namespace Api.Campaign.Crm
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public static ApplicationSettings Settings { get; set; }
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container
        public void ConfigureServices(IServiceCollection services)
        {
            //const string campaignScope = "campaign";

            var settings = Settings ?? Configuration.GetApplicationSettings();

            services.AddDefaultApplicationSettings(settings);

            services.AddAuthenticationService(settings.Authentication);

            services.AddControllers(op =>
            {
                var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();

                op.Filters.Add(new AuthorizeFilter(policy));
                op.Filters.Add(new GlobalValidateModelStateFilter());
            });

            services.AddTransient<IAuthorizationHandler, ScopeAuthorizationHandler>();
            services.AddTransient<IAudienceManagerService, AudienceManagerService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline
        public void Configure(IApplicationBuilder app, ApplicationSettings settings)
        {
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor
            });
            app.UseRequestLogging();
            app.UseSerilogRequestLogging();
            app.UseDefaultCorsPolicy(settings.Cors);
            app.UseAuthentication();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
                endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
            });
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Campaign API");
                c.OAuthClientId("crm_campaign_api");
            });
        }
    }
}
