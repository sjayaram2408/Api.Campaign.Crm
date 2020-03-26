using System.Collections.Generic;

namespace Api.Campaign.Crm
{
    public class LoggingSettings
    {
        public bool IncludeRequestBody { get; set; }
        public IEnumerable<string> RequestBodyFilters { get; set; }
    }
}
