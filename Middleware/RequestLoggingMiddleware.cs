using Api.Campaign.Crm.Configuration;
using Api.Campaign.Crm.Logging.Enrichers;
using Microsoft.AspNetCore.Http;
using Serilog;
using Serilog.Context;
using Serilog.Events;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Campaign.Crm.Middleware
{
    public class RequestLoggingMiddleware
    {
        private const string MessageTemplate = "HTTP {cs_method} {cs_uri_stem} responded {sc_status} in {time_taken:0} ms";

        private static readonly ILogger Log = Serilog.Log.ForContext<RequestLoggingMiddleware>();

        private readonly RequestDelegate _next;
        private readonly LoggingSettings _settings;

        public RequestLoggingMiddleware(RequestDelegate next)
            : this(next, null)
        {
        }

        public RequestLoggingMiddleware(RequestDelegate next, LoggingSettings settings)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _settings = settings;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var startTime = DateTime.UtcNow;
            var sw = new Stopwatch();
            sw.Start();

            try
            {
                await _next(context);

                var elapsedMs = sw.ElapsedMilliseconds;

                var statusCode = context.Response?.StatusCode;
                var level = statusCode >= 500 ? LogEventLevel.Error : LogEventLevel.Information;

                using (LogContext.Push(new W3CRequestEnricher(context, startTime, elapsedMs, _settings)))
                {
                    var log = level == LogEventLevel.Error ? LogForErrorContext(context) : Log;
                    log.Write(level, MessageTemplate, context.Request.Method, context.Request.Path, statusCode, elapsedMs);
                }
            }

            // Never caught, because `LogException()` returns false.
            catch (Exception ex) when (LogException(context, sw.ElapsedMilliseconds, ex))
            {
            }
        }

        private static bool LogException(HttpContext httpContext, double elapsedMs, Exception ex)
        {
            LogForErrorContext(httpContext)
                .Error(ex, MessageTemplate, httpContext.Request.Method, httpContext.Request.Path, 500, elapsedMs);

            return false;
        }

        private static ILogger LogForErrorContext(HttpContext httpContext)
        {
            var request = httpContext.Request;

            var result = Log
                .ForContext("RequestHeaders", request.Headers.ToDictionary(h => h.Key, h => h.Value.ToString()), true)
                .ForContext("RequestHost", request.Host)
                .ForContext("RequestProtocol", request.Protocol);

            if (request.HasFormContentType)
            {
                result = result.ForContext("RequestForm", request.Form.ToDictionary(v => v.Key, v => v.Value.ToString()));
            }

            return result;
        }
    }
}
