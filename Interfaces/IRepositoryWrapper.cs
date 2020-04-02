using Api.Campaign.Crm.RepositoryClasses.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Campaign.Crm.Interfaces
{
    public interface IRepositoryWrapper
    {
        ISiteAudienceRepository SiteAudience { get; }
        void Save();
    }
}
