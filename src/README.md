
# Serilog.Sinks.AspNetCore.App.SignalR

[![NuGet Version](https://img.shields.io/nuget/v/Serilog.Sinks.AspNetCore.App.SignalR.svg)](https://www.nuget.org/packages/Serilog.Sinks.AspNetCore.App.SignalR/) [![NuGet Downloads](https://img.shields.io/nuget/dt/Serilog.Sinks.AspNetCore.App.SignalR.svg)](https://www.nuget.org/packages/Serilog.Sinks.AspNetCore.App.SignalR/) 

- [Quickstart](#quickstart)
    * [Register with default hub](#register-with-default-hub)
    * [Register with user defined hub](#register-with-user-defined-hub)
    * [Register with configuration](#register-with-configuration)

# Quickstart

A short and sweet overview of how to register `Serilog.Sinks.AspNetCore.App.SignalR` to help you get up and running. There are a few methods of dependency injection registration. You should choose the one appropriate for your situation and how much flexibility you might require.

## Register with default hub

This is by far the simplest way to integrate Serilog with SignalR. Add a call to the `IServiceCollection` extension method `AddDefaultSerilogHub`. This will register the `DefaultSerilogHub` from this package.

    // Register the default SignalR Hub for Serilog.
    builder.Services.AddDefaultSerilogHub();

> [!NOTE]
> The `DefaultSerilogHub` is just an empty class that inherits from `Microsoft.AspNetCore.SignalR.Hub`. 

Then, configure the logger by passing the method name that the SignalR client is listening to.

    builder.Services.AddSerilog((serviceProvider, loggerConfiguration) => 
        loggerConfiguration.WriteTo.SignalR(
            serviceProvider, 
            "ReceiveEvent"
        ));

Finally, call the `WebApplication` extension method `MapDefaultSerilogHub` and specify the route for the `DefaultSerilogHub`.

    app.MapDefaultSerilogHub("/sample");

A client can now listen on the `Hub` route for the specified method where the formatted message will be sent.

    hub.on("ReceiveEvent", (message) => {...});

Additionally, the Serilog `LogEvent` is also passed as an optional second parameter.

    hub.on("ReceiveEvent", (message, logEvent) => {...});

## Register with user defined hub

Call the `IServiceCollection` extension method `AddSerilogHub` to register a SignalR `Hub` with the Serilog sink. 

> [!IMPORTANT]
> This step is necessary in order to prevent circular dependencies caused during logger initialization.

    builder.Services.AddSerilogHub<SampleHub>();

Call `AddSerilog` and configure the `Hub` in which events should be logged to. Make sure to pass the `IServiceProvider` down to the SignalR sink. The delegate provides the `IHubContext` for the `Hub` specified in the generic type parameter of the SignalR sink and the formatted Serilog event message.

    builder.Services.AddSerilog(
        (serviceProvider, loggerConfiguration) => 
            loggerConfiguration.WriteTo.SignalR<SampleHub>(
                serviceProvider, 
                (context, message, logEvent) => 
                    context.Clients.All.SendAsync("ReceiveEvent", message, logEvent)
            ));

Lastly, make sure to register the `Hub`.

    app.MapHub<SampleHub>("/sample");

## Register with configuration

The setup for this method is the same as [registering a default hub](#register-with-default-hub). The only difference is the call to `AddSerilog` where an `IConfiguration` is utilized.

    services.AddSerilog((serviceProvider, options) => 
    {
        var config = serviceProvider.GetRequiredService<IConfiguration>();
        options.ReadFrom.Configuration(config)
            .WriteTo.SignalR(serviceProvider, config);
    });

The expected format for configuration is as follows.

    {
        "Serilog": {
            "Using": [
                "Serilog.Sinks.AspNetCore.App.SignalR"
            ],
            "WriteTo": [
                {
                    "Name": "SignalR",
                    "Args": {
                        "HubMethod": "ReceiveLogEvent"
                    }
                }
            ]
        }
    }

> [!NOTE]
> The only available argument for configuration at this moment is the hub method name.