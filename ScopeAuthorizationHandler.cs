using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace Api.Campaign.Crm
{ 
    public class ScopeAuthorizationHandler : AuthorizationHandler<ScopeRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ScopeRequirement requirement)
        {
            const string ScopeClaim = "scope";
            foreach (var requiredScope in requirement.Scopes)
            {
                if (context.User.HasClaim(ScopeClaim, requiredScope))
                {
                    context.Succeed(requirement);
                    break;
                }
            }

            return Task.CompletedTask;
        }
    }
}
