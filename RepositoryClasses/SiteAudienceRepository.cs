using Api.Campaign.Crm.Context;
using Api.Campaign.Crm.Helpers;
using Api.Campaign.Crm.Models;
using Api.Campaign.Crm.RepositoryClasses.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Campaign.Crm.RepositoryClasses
{
    public class SiteAudienceRepository : RepositoryBase<SiteAudience>, ISiteAudienceRepository
    {
        public SiteAudienceRepository(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {
        }
    }
}
