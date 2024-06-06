
# Serilog.Sinks.AspNetCore.App.SignalR

[![NuGet Version](https://img.shields.io/nuget/v/Serilog.Sinks.AspNetCore.App.SignalR.svg)](https://www.nuget.org/packages/Serilog.Sinks.AspNetCore.App.SignalR/) [![NuGet Downloads](https://img.shields.io/nuget/dt/Serilog.Sinks.AspNetCore.App.SignalR.svg)](https://www.nuget.org/packages/Serilog.Sinks.AspNetCore.App.SignalR/) 

## Summary

An easy-to-use Serilog sink that enables logging to SignalR in ASP.NET Core applications.

## Usage

Call the `IServiceCollection` extension method `AddSerilogHub` to register a SignalR `Hub` with the Serilog sink. This step is necessary in order to prevent circular dependencies caused during logger initialization.

    builder.Services.AddSerilogHub<SampleHub>();

Call `AddSerilog` and configure the `Hub` in which events should be logged to. Make sure to pass the `IServiceProvider` down to the SignalR sink. The delegate provides the `IHubContext` for the `Hub` specified in the generic type parameter of the SignalR sink and the formatted Serilog event message.

    builder.Services.AddSerilog(
        (serviceProvider, loggerConfiguration) => 
            loggerConfiguration.WriteTo.SignalR<SampleHub>(
                serviceProvider, 
                (context, message) => 
                    context.Clients.All.SendAsync("Receive", message)
            ));

Lastly, make sure to register the `Hub`.

    app.MapHub<SampleHub>("/sample");

## Installation

`Serilog.Sinks.AspNetCore.App.SignalR` is available on [NuGet](https://www.nuget.org/packages/Serilog.Sinks.AspNetCore.App.SignalR/). 

    dotnet add package Serilog.Sinks.AspNetCore.App.SignalR
