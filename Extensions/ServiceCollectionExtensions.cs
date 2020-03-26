using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;

namespace Api.Campaign.Crm
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAuthenticationService(this IServiceCollection collection, AuthenticationSettings settings)
        {
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            collection.AddAuthentication(o =>
            {
                o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o =>
            {
                o.Authority = settings.Authority;
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.FromSeconds(10),
                    NameClaimType = "account_username",
                    RoleClaimType = "admin_role"
                };
            });

            return collection;
        }

    }
}