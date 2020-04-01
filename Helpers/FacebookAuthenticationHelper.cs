using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Api.Campaign.Crm.Helpers
{
    internal class AuthToken
    {
        [JsonProperty(PropertyName = "access_token")]
        public string AccessToken { get; set; }

        [JsonProperty(PropertyName = "expires_in")]
        public int ExpiresIn { get; set; }

        [JsonProperty(PropertyName = "token_type")]
        public string TokenType { get; set; }
    }

    public class FacebookAuthenticationHelper
    {
        private readonly string _clientId = string.Empty;
        private readonly string _clientSecret = string.Empty;
        private readonly string _scopes = string.Empty;
        private readonly string _uriPath = string.Empty;

        public FacebookAuthenticationHelper(IConfiguration configuration)
        {
            _clientId = configuration["ApplicationSettings:Facebook:ClientId"];
            _clientSecret = configuration["ApplicationSettings:Facebook:ClientSecret"];
            _scopes = configuration["ApplicationSettings:Facebook:Scopes"];
            _uriPath = configuration["ApplicationSettings:Facebook:GetTokenURL"];
        }
        public async Task<string> GetAccessTokenForFacebookMilestonesAsync()
        {
            using (var client = new HttpClient())
            {
                using (var content = new FormUrlEncodedContent(GetFormData()))
                {
                    content.Headers.Clear();
                    content.Headers.Add("Content-Type", "application/x-www-form-urlencoded");

                    var result = await client.PostAsync(new Uri(_uriPath), content).ConfigureAwait(false);
                    if (result.IsSuccessStatusCode)
                    {
                        var stringToParse = await result.Content.ReadAsStringAsync().ConfigureAwait(false);
                        var token = JsonConvert.DeserializeObject<AuthToken>(stringToParse);
                        return token.AccessToken;
                    }

                    throw new Exception(
                        $"Reason: {result.ReasonPhrase}, Content: {await result.Content.ReadAsStringAsync().ConfigureAwait(false)}");
                }
            }
        }

        private List<KeyValuePair<string, string>> GetFormData()
        {
            var contentList = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("grant_type", "client_credentials"),
                new KeyValuePair<string, string>("scope", _scopes),
                new KeyValuePair<string, string>("client_id", _clientId),
                new KeyValuePair<string, string>("client_secret", _clientSecret)
            };
            return contentList;
        }
    }
}
