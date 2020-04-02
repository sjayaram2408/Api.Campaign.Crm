using Api.Campaign.Crm.Models;

namespace Api.Campaign.Crm.Services.Interfaces
{
    public interface IAudienceManagerService
    {
        FacebookAudienceResponse CreateCustomAudienceIntegration(FacebookAudienceManager audienceManager);

    }
}