using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using Serilog.Core;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Api.Campaign.Crm
{
    // https://www.w3.org/TR/WD-logfile.html
    internal class W3CRequestEnricher : ILogEventEnricher
    {
        private readonly HttpContext _context;
        private readonly long _duration;
        private readonly DateTime _startTime;
        private readonly LoggingSettings _loggingSettings;

        public W3CRequestEnricher(HttpContext context, DateTime startTime, long duration, LoggingSettings loggingSettings)
        {
            _context = context;
            _startTime = startTime;
            _duration = duration;
            _loggingSettings = loggingSettings;
        }

        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            var fields = new Dictionary<string, object>
            {
                { "@timestamp", _startTime },
                { "s_ip", _context.Connection.LocalIpAddress?.ToString() },
                { "cs_method", _context.Request.Method },
                { "cs_uri_stem", _context.Request.Path },
                { "cs_uri_query", _context.Request.QueryString.ToString() },
                { "cs_user_agent", _context.Request.Headers.ContainsKey("User-Agent") ? _context.Request.Headers["User-Agent"].ToString() : null },
                { "cs_username", _context.User.Identity?.Name },
                { "cs_referer", _context.Request.Headers.ContainsKey("Referer") ? _context.Request.Headers["Referer"].ToString() : null },
                { "c_ip", _context.Connection.RemoteIpAddress?.ToString() },
                { "cs_host", _context.Request.Host.Value },
                { "sc_status", _context.Response.StatusCode },
                { "time_taken", _duration }
            };

            try
            {
                fields.Add("cs_bytes", _context.Request.ContentLength ?? _context.Request.Body.Length);

                if (_loggingSettings?.IncludeRequestBody == true)
                {
                    var requestBody = GetRequestBody();
                    if (_loggingSettings.RequestBodyFilters == null || !_loggingSettings.RequestBodyFilters.Any())
                    {
                        fields.Add("request_body", requestBody);
                    }
                    else
                    {
                        var jsonObject = JObject.Parse(requestBody);
                        if (jsonObject != null)
                        {
                            var jsonFields = _loggingSettings
                                .RequestBodyFilters
                                .Where(f => jsonObject.ContainsKey(f));

                            foreach (var jsonField in jsonFields)
                            {
                                fields.Add(jsonField, (string)jsonObject[jsonField]);
                            }
                        }
                    }
                }

                fields.Add("sc_bytes", _context.Response.ContentLength ?? _context.Response.Body.Length);
            }
            catch (Exception)
            {
                // Ignore
            }

            foreach (var field in fields)
            {
                logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(field.Key, field.Value));
            }
        }

        private string GetRequestBody()
        {
            _context.Request.Body.Position = 0;
            using (var streamReader = new StreamReader(_context.Request.Body))
            {
                return streamReader.ReadToEnd();
            }
        }
    }
}
