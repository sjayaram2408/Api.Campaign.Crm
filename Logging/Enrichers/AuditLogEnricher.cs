using Audit.Core;
using Audit.WebApi;
using Serilog.Core;
using Serilog.Events;
using System;
using System.Collections.Generic;

namespace Api.Campaign.Crm
{
    internal class AuditLogEnricher : ILogEventEnricher
    {
        private readonly AuditEvent _auditEvent;
        private readonly AuditApiAction _action;

        public AuditLogEnricher(AuditEvent auditEvent, AuditApiAction action)
        {
            _auditEvent = auditEvent;
            _action = action;
        }

        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("raw", _auditEvent.ToJson()));

            var fields = new Dictionary<string, object>
            {
                { "@timestamp", DateTime.UtcNow },
                { "cs_method", _action.HttpMethod },
                { "cs_uri_stem", _action.RequestUrl },
                { "cs_user_agent", _action.Headers.ContainsKey("User-Agent") ? _action.Headers["User-Agent"] : null },
                { "cs_username", _action.UserName },
                { "cs_referer", _action.Headers.ContainsKey("Referer") ? _action.Headers["Referer"] : null },
                { "c_ip", _action.IpAddress },
                { "cs_host", _action.Headers.ContainsKey("Host") ? _action.Headers["Host"] : null },
                { "sc_status", _action.ResponseStatusCode },
                { "time_taken", _auditEvent.Duration },
                { "cs_bytes", _action.RequestBody?.Length }
            };

            foreach (var field in fields)
            {
                logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(field.Key, field.Value));
            }
        }
    }
}