using Api.Campaign.Crm.Models;
using System.Threading.Tasks;

namespace Api.Campaign.Crm.Services.Interfaces
{
    public interface IAudienceManagerThirdParty
    {
        Task<FacebookAudienceResponse> CreateCustomAudienceIntegration(FacebookAudienceManager audienceManager);

    }
}