# MvcWithDefaultHubs Sample

This is a sample project using the simplest method for wiring up Serilog events to SignalR for client consumption.

#### Note

For information on configuring the `Program.cs`, visit [here](../../src/README.md).

## Usage

This is by far the simplest way to integrate Serilog with SignalR. Add a call to the `IServiceCollection` extension method `AddDefaultSerilogHub`. This will register the internal SignalR `Hub` from this package.

    // Register the default SignalR Hub for Serilog.
    builder.Services.AddDefaultSerilogHub();

Then, configure the logger by passing the method name that the SignalR client is listening to.

    builder.Services.AddSerilog((serviceProvider, loggerConfiguration) => 
        loggerConfiguration.WriteTo.SignalR(
            serviceProvider, 
            "ReceiveEvent"
        ));

Finally, call the `WebApplication` extension method `MapDefaultSerilogHub` and specify the route for the internal, default `Hub`.

    app.MapDefaultSerilogHub("/sample");

A client can now listen on the `Hub` route for the specified method where the formatted message will be sent.

    hub.on("ReceiveEvent", (message) => {...});

Additionally, the Serilog `LogEvent` is also passed as an optional second parameter.

    hub.on("ReceiveEvent", (message, logEvent) => {...});