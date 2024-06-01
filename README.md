
# Serilog.Sinks.AspNetCore.App.SignalR

[![NuGet Version](https://img.shields.io/nuget/v/Serilog.Sinks.AspNetCore.App.SignalR.svg)](https://www.nuget.org/packages/Serilog.Sinks.AspNetCore.App.SignalR/) 

## Summary

An easy to use Serilog sink for SignalR.

## Usage

- Call `AddSerilogHub` to register a `Hub` to be recognized by the Serilog sink.
- Call `AddSerilog` and configure the `Hub` to log events to.
- Pass the `IServiceProvider` and the `Hub` method to be used to write out the events.

        builder.Services.AddSerilogHub<SampleHub>();
        builder.Services.AddSerilog(
            (serviceProvider, loggerConfiguration) => 
                loggerConfiguration.WriteTo.SignalR<SampleHub>(
                    serviceProvider, 
                    (context, message) => 
                        context.Clients.All.SendAsync("Receive", message)
                ));

- Don't forget to register the `Hub`

        app.MapHub<SampleHub>("/sample");

## Installation
`Serilog.Sinks.AspNetCore.App.SignalR` is available on [NuGet](https://www.nuget.org/packages/Serilog.Sinks.AspNetCore.App.SignalR/). 

    dotnet add package Serilog.Sinks.AspNetCore.App.SignalR


