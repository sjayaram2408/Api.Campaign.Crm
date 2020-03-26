using System.Collections.Generic;

namespace Api.Campaign.Crm
{
    public class SwaggerSettings
    {
        public string OAuth2Authority { get; set; }
        public IDictionary<string, string> OAuth2Scopes { get; set; }
        public string DocumentTitle { get; set; }
        public string DocumentVersion { get; set; }
        public string DocumentationLocation { get; set; }
    }
}
