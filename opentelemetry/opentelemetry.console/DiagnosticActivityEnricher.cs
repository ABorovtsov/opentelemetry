using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Serilog.Core;
using Serilog.Events;

namespace opentelemetry.console
{
    public class DiagnosticActivityEnricher : ILogEventEnricher
    {
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            var activity = Activity.Current;

            if (activity is null) return;

            logEvent.AddPropertyIfAbsent(new LogEventProperty("Activity.Id", new ScalarValue(activity.Id)));
            logEvent.AddPropertyIfAbsent(new LogEventProperty("Activity.ParentId", new ScalarValue(activity.ParentId)));
            logEvent.AddPropertyIfAbsent(new LogEventProperty("Activity.Tags", 
                new StructureValue(new List<LogEventProperty>(activity.Tags.Select(t => new LogEventProperty(t.Key, new ScalarValue(t.Value)))))));
            logEvent.AddPropertyIfAbsent(new LogEventProperty("Activity.Events",
                new SequenceValue(
                    new List<LogEventPropertyValue>(activity.Events.Select(e => new ScalarValue(e.Name))))));
            logEvent.AddPropertyIfAbsent(new LogEventProperty("Activity.Baggage",
                new StructureValue(new List<LogEventProperty>(activity.Baggage.Select(b => new LogEventProperty(b.Key, new ScalarValue(b.Value)))))));
        }
    }
}