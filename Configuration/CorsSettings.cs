using System.Collections.Generic;

namespace Api.Campaign.Crm
{
    public class CorsSettings
    {
        public IEnumerable<string> AllowedOrigins { get; set; }

        /// <summary>
        /// Gets or sets the AllowedOriginsString value.
        /// ATTN: This value is not used directly. If it is set via Environment variable, you must split the string and set the AllowedOrigins property.
        /// </summary>
        /// <value>
        /// The AllowedOriginsString value.
        /// </value>
        public string AllowedOriginsString { get; set; }
    }
}
