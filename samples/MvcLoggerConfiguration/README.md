# MvcLoggerConfiguration Sample

This sample shows all of the possible ways to configure a Serilog sink using a SignalR `Hub`.

#### Note

For information on configuring the `Program.cs`, visit [here](../../src/README.md).

## Usage

### Method 1

Register the default SignalR Hub for Serilog. This method doesn't require any hubs to exist in the consuming codebase a default one is registered internally.

    builder.Services.AddDefaultSerilogHub();
    builder.Services.AddSerilog((serviceProvider, loggerConfiguration) => 
        loggerConfiguration.WriteTo.SignalR(
            serviceProvider, 
            "ReceiveEvent"
        ));

Then call the `WebApplication` extension method `MapDefaultSerilogHub` to define the route where the `Hub` will exist.

    app.MapDefaultSerilogHub("/sample");

### Method 2

Register your own hub for Serilog and specify the hub method used to push events.

    builder.Services.AddSerilogHub<SampleHub>();
    builder.Services.AddSerilog((serviceProvider, loggerConfiguration) =>
        loggerConfiguration.WriteTo.SignalR<SampleHub>(
            serviceProvider, 
            "ReceiveEvent"
        ));

Then call `MapHub` as usual.

    app.MapHub<SampleHub>("/sample");

### Method 3

Register your own hub for Serilog and define the delegate to push events. This allows the most flexibility as you have direct access to the context representing the specified hub. There is also an overload that accepts the Serilog event for further processing.

    builder.Services.AddSerilogHub<SampleHub>();
    builder.Services.AddSerilog((serviceProvider, loggerConfiguration) =>
        loggerConfiguration.WriteTo.SignalR<SampleHub>(
            serviceProvider, 
            (context, message) => context.Clients.All.SendAsync(...)
            //(context, message, logEvent) => context.Clients.All.SendAsync(...)
        ));

Then call `MapHub` as usual.

    app.MapHub<SampleHub>("/sample");