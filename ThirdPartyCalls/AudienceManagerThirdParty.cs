using Api.Campaign.Crm.Models;
using Api.Campaign.Crm.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Api.Campaign.Crm.ThirdPartyCalls
{
    public class AudienceManagerThirdParty : IAudienceManagerThirdParty
    {
        private static readonly HttpClient HttpClient = new HttpClient();
        private readonly string _accessToken = string.Empty;
        private readonly string _appSecret = string.Empty;

        public AudienceManagerThirdParty(IConfiguration configuration)
        {
            _accessToken = configuration["ApplicationSettings:Facebook:AccessToken"];
            _appSecret = configuration["ApplicationSettings:Facebook:AppSecret"];
        }

        public async Task<string> CreateCustomAudience(FacebookAudienceManager audienceManager)
        {
            if (audienceManager?.Account == null)
            {
                throw new Exception("null audienceManager or audienceManager.Account");
            }

            audienceManager.Account.AccessToken = _accessToken;

            var values = new Dictionary<string, string>
            {
                { "name", audienceManager.Name},
                { "subtype", "CUSTOM" },
                { "description", audienceManager.Description},
                { "customer_file_source", "USER_PROVIDED_ONLY" },
                { "access_token", audienceManager.Account.AccessToken },
                { "retention_days","60"},
                { "fields","id,name,retention_days,time_created"}
            };
            var content = new FormUrlEncodedContent(values);

            if (audienceManager.UseMock != true)
            {
                var response = await HttpClient.PostAsync("https://" + $"graph.facebook.com/v5.0/act_{audienceManager.Account.Id}/customaudiences", content);
                return await response.Content.ReadAsStringAsync();
            }

            const string fakeOutput = "{\"success\":true}"; //this is what they really respond with
            return await Task.FromResult(fakeOutput);
        }
    }
}