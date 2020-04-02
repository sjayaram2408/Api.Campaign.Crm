using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Campaign.Crm.Models
{
    public class FacebookAudienceResponse
    {
        [JsonProperty(PropertyName = "id")]
        public long AudienceId { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string AudienceName { get; set; }

        [JsonProperty(PropertyName = "retention_days")]
        public int RetentionDays { get; set; }

        [JsonProperty(PropertyName = "time_created")]
        public long CreatedDate { get; set; }

        [JsonProperty(PropertyName = "time_updated")]
        public long UpdatedDate { get; set; }

        [JsonProperty(PropertyName = "error")]
        public string Error { get; set; }

        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }
    }
}
