using Microsoft.AspNetCore.Mvc.Filters;

namespace Api.Campaign.Crm
{
    /// <inheritdoc />
    /// <summary>
    /// An attribute for disabling the Global ModelState Validation filter.
    /// </summary>
    public class DisableValidateModelStateFilterAttribute : ActionFilterAttribute
    {
    }
}
