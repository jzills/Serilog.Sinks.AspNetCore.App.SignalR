using Serilog.Core;
using Serilog.Events;

namespace MvcWithCustomLogProperties.Enrichers;

// Our UserEnricher to add custom properties
// to our log events...
public class UserEnricher : ILogEventEnricher
{
    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        // Because...jazz
        logEvent.AddOrUpdateProperty(
            propertyFactory.CreateProperty("Name", "John Coltrane"));

        logEvent.AddOrUpdateProperty(
            propertyFactory.CreateProperty("Email", "john.coltrane@alovesupreme.com")); // Pt. 2: Resolution 
    }
}