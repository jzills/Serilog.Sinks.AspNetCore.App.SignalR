# MvcWithWithCustomLogProperties Sample

This is a sample project demonstrating how custom properties in a Serilog `LogEvent` can be retrieved in a SignalR `Hub`.

#### Note

For information on configuring the `Program.cs`, visit [here](../../src/README.md).

## Usage

See [here](../Mvc/README.md) for initial setup details. The only difference is that we are utilizing the Serilog `LogEvent` that is passed to our SignalR `Hub`.

    builder.Services.AddSerilog((serviceProvider, loggerConfiguration) => 
        loggerConfiguration.WriteTo.SignalR<SampleHub>(
            serviceProvider, 
            (context, message, logEvent) =>
            {
                // Only push events with our custom properties
                var properties = logEvent.Properties;
                if (properties.TryGetValue("Name",  out var nameProperty) &&
                    properties.TryGetValue("Email", out var emailProperty))
                {
                    string name  = // Write property value
                    string email = // Write property value
                    
                    // Push the formatted message with our custom properties
                    return context.Clients.All.SendAsync(
                        "LogUser", 
                        message, 
                        name, 
                        email
                    );
                }
                ...
            }
        ));