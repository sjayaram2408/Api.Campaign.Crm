using Api.Campaign.Crm.Models;
using Api.Campaign.Crm.Services.Interfaces;
using System;

namespace Api.Campaign.Crm.Services
{
    public class AudienceManagerService : IAudienceManagerService
    {
        private IAudienceManagerThirdParty _audienceManagerThirdParty;

        public AudienceManagerService(IAudienceManagerThirdParty audienceManagerThirdParty)
        {
            _audienceManagerThirdParty = audienceManagerThirdParty;
        }

        public string CreateCustomAudience(FacebookAudienceManager audienceManager)
        {
            if (audienceManager.Account == null)
            {
                throw new Exception("null account");
            }

            if (audienceManager.AddressId < 1)
            {
                throw new Exception("invalid addressId");
            }

            if (audienceManager.SiteId < 1)
            {
                throw new Exception("invalid siteId");
            }

            if (string.IsNullOrWhiteSpace(audienceManager.Description))
            {
                throw new Exception("invalid Description");
            }

            if (string.IsNullOrWhiteSpace(audienceManager.Name))
            {
                throw new Exception("invalid Name");
            }

            if (string.IsNullOrWhiteSpace(audienceManager.Account.Id))
            {
                throw new Exception("invalid Id (ad account id)");
            }

            return _audienceManagerThirdParty.CreateCustomAudience(audienceManager).Result;
        }

        public string CreateCustomAudienceIntegration(FacebookAudienceManager audienceManager)
        {
            if (audienceManager.Account == null)
            {
                throw new Exception("null account");
            }

            if (audienceManager.AddressId < 1)
            {
                throw new Exception("invalid addressId");
            }

            if (audienceManager.SiteId < 1)
            {
                throw new Exception("invalid siteId");
            }

            if (string.IsNullOrWhiteSpace(audienceManager.Description))
            {
                throw new Exception("invalid Description");
            }

            if (string.IsNullOrWhiteSpace(audienceManager.Name))
            {
                throw new Exception("invalid Name");
            }

            if (string.IsNullOrWhiteSpace(audienceManager.Account.Id))
            {
                throw new Exception("invalid Id (ad account id)");
            }

            return _audienceManagerThirdParty.CreateCustomAudienceIntegration(audienceManager).Result;
        }
    }
}