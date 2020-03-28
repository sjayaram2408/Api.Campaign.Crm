using System.Collections.Generic;

namespace Api.Campaign.Crm.Configuration
{
    public class LoggingSettings
    {
        public bool IncludeRequestBody { get; set; }
        public IEnumerable<string> RequestBodyFilters { get; set; }
    }
}
