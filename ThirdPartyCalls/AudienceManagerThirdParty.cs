using Api.Campaign.Crm.Helpers;
using Api.Campaign.Crm.Models;
using Api.Campaign.Crm.Services.Interfaces;
using IdentityModel.Client;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Api.Campaign.Crm.ThirdPartyCalls
{
    public class AudienceManagerThirdParty : IAudienceManagerThirdParty
    {
        private static readonly HttpClient HttpClient = new HttpClient();
        private readonly string _accessToken = string.Empty;
        private readonly string _appSecret = string.Empty;
        private readonly IConfiguration _configuration;

        public AudienceManagerThirdParty(IConfiguration configuration)
        {
            _accessToken = configuration["ApplicationSettings:Facebook:AccessToken"];
            _appSecret = configuration["ApplicationSettings:Facebook:AppSecret"];
            _configuration = configuration;
        }

        public async Task<FacebookAudienceResponse> CreateCustomAudienceIntegration(FacebookAudienceManager audienceManager)
        {
            if (audienceManager?.Account == null)
            {
                throw new Exception("null audienceManager or audienceManager.Account");
            }
            var facebookAuthenticaionHelper = new FacebookAuthenticationHelper(_configuration);
            var token = await facebookAuthenticaionHelper.GetAccessTokenForFacebookMilestonesAsync()
                .ConfigureAwait(false);
            HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var stringContent = new StringContent(JsonConvert.SerializeObject(audienceManager), Encoding.UTF8, "application/json");
            var response = await HttpClient.PostAsync("https://iapi.local.dealersocket.com/FacebookAudienceManager", stringContent).ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
            {
                var stringToParse = JsonConvert.DeserializeObject<string>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
                var customAudience = JsonConvert.DeserializeObject<FacebookAudienceResponse>(stringToParse);
                return customAudience;
            }

            throw new Exception(
                $"Reason: {response.ReasonPhrase}, Content: {await response.Content.ReadAsStringAsync().ConfigureAwait(false)}");
        }
    }
}