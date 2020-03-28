using Api.Campaign.Crm.Logging.Enrichers;
using Audit.Core;
using Audit.WebApi;
using Serilog;
using Serilog.Context;

namespace Api.Campaign.Crm.Logging
{
    public class AuditLogCloudWatchProvider : AuditDataProvider
    {
        private const string MessageTemplate = "AUDIT_EVENT {event_type} {cs_uri_stem} responded {response_status} in {time_taken:0} ms";

        private static readonly ILogger Log = Serilog.Log.ForContext<AuditLogCloudWatchProvider>();

        public override object InsertEvent(AuditEvent auditEvent)
        {
            var action = auditEvent.GetWebApiAuditAction();

            using (LogContext.Push(new AuditLogEnricher(auditEvent, action)))
            {
                Log.Information(MessageTemplate, auditEvent.EventType, action.RequestUrl, action.ResponseStatus, auditEvent.Duration);
            }

            return null;
        }
    }
}
