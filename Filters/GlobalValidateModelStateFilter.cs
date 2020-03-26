using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Campaign.Crm
{
    /// <inheritdoc />
    /// <summary>
    /// A Global ModelState Validation filter which should be added in the services section of Startup.cs.
    /// </summary>
    public class GlobalValidateModelStateFilter : ActionFilterAttribute
    {
        public override Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.ModelState.IsValid && !context.Filters.Any(i => i is DisableValidateModelStateFilterAttribute))
            {
                context.Result = new BadRequestObjectResult(context.ModelState);
            }

            return base.OnActionExecutionAsync(context, next);
        }
    }
}
