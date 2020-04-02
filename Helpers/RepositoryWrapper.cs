using Api.Campaign.Crm.Context;
using Api.Campaign.Crm.Interfaces;
using Api.Campaign.Crm.RepositoryClasses;
using Api.Campaign.Crm.RepositoryClasses.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Campaign.Crm.Helpers
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private RepositoryContext _repoContext;
        private ISiteAudienceRepository _siteAudience;

        public ISiteAudienceRepository SiteAudience
        {
            get
            {
                if (_siteAudience == null)
                {
                    _siteAudience = new SiteAudienceRepository(_repoContext);
                }

                return _siteAudience;
            }
        }

        public RepositoryWrapper(RepositoryContext repositoryContext)
        {
            _repoContext = repositoryContext;
        }

        public void Save()
        {
            _repoContext.SaveChanges();
        }
    }
}
