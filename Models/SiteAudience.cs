using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Campaign.Crm.Models
{
    public class SiteAudience
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long SiteAudienceId { get; set; }
        public int SiteId { get; set; }
        public long AudienceId { get; set; }
        public int RetentionDays { get; set; }
        public string AudienceName { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
