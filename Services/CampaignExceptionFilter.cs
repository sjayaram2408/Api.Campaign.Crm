using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Net;

namespace Api.Campaign.Crm
{
    public class CampaignExceptionFilter : IExceptionFilter
    {
        private readonly ILogger _logger;

        public CampaignExceptionFilter(ILogger logger)
        {
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            var status = HttpStatusCode.InternalServerError;
            var exception = context.Exception;
            var message = exception.Message;

            if (exception is ArgumentOutOfRangeException ||
                exception is NotSupportedException)
            {
                status = HttpStatusCode.BadRequest;
                _logger.LogInformation(exception, message);
            }
            else if (exception is NotImplementedException)
            {
                status = HttpStatusCode.NotImplemented;
                _logger.LogError(exception, message);
            }
            else
            {
                _logger.LogError(exception, message);
            }

            context.ExceptionHandled = true;

            var response = context.HttpContext.Response;
            response.StatusCode = (int)status;
            response.WriteAsync(message);
        }
    }
}
