using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;

namespace Api.Campaign.Crm.Configuration
{
    public class ScopeRequirement : IAuthorizationRequirement
    {
        public ScopeRequirement(ICollection<string> scopes)
        {
            Scopes = scopes;
        }

        public ICollection<string> Scopes { get; set; }
    }
}
