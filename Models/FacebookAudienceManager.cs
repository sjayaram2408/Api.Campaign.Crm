using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Campaign.Crm.Models
{
    public class FacebookAudienceManager
    {
        public FacebookAccount Account { get; set; }
        public int AddressId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int SiteId { get; set; }
        public bool UseMock { get; set; }
        public string RetentionDays { get; set; }
    }
}
