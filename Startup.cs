using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DealerSocket.AspNetHosting.Mvc.Extensions;
using DealerSocket.AspNetHosting.Mvc.Filters;
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

namespace Api.Campaign.Crm
{
    public class Startup
    {
        public Startup(IConfiguration configuration, Microsoft.AspNetCore.Hosting.IHostingEnvironment env)
        {
            Configuration = configuration;
        }

        public static ApplicationSettings Settings { get; set; }
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container
        public void ConfigureServices(IServiceCollection services)
        {
            const string apiScope = "crm_api";
            //const string campaignScope = "campaign";

            var settings = Settings ?? Configuration.GetApplicationSettings();

            services.AddMvc(options => options.EnableEndpointRouting = false);

            services.AddDefaultApplicationSettings(settings);

            services.AddAuthenticationService(settings.Authentication);

            services.AddMvc(op =>
            {
                var policy = new AuthorizationPolicyBuilder()
                    .AddRequirements(new ScopeRequirement(new[] { apiScope }))
                    .Build();

                op.Filters.Add(new AuthorizeFilter(policy));
                op.Filters.Add(new GlobalValidateModelStateFilter());
            });

            services.AddSwaggerService(settings.Swagger);

            // Authentication
            services.AddTransient<IAuthorizationHandler, ScopeAuthorizationHandler>();

            services.AddMemoryCache();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline
        public void Configure(IApplicationBuilder app, ApplicationSettings settings)
        {
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor
            });
            app.UseRequestLogging(settings.Logging);
            app.UseDefaultCorsPolicy(settings.Cors);
            app.UseAuthentication();
            app.UseMvc();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Campaign API");
                c.OAuthClientId("crm_campaign_api");
            });
        }
    }
}
