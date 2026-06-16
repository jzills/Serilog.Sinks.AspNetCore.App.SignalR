<div align="center">
  <img src="resources/serilog-banner.svg" alt="Serilog.Sinks.AspNetCore.App.SignalR" width="100%" />
</div>

<div align="center">

[![NuGet Version](https://img.shields.io/nuget/v/Serilog.Sinks.AspNetCore.App.SignalR.svg)](https://www.nuget.org/packages/Serilog.Sinks.AspNetCore.App.SignalR/)
[![NuGet Downloads](https://img.shields.io/nuget/dt/Serilog.Sinks.AspNetCore.App.SignalR.svg)](https://www.nuget.org/packages/Serilog.Sinks.AspNetCore.App.SignalR/)
![Target Frameworks](https://img.shields.io/badge/targets-net8.0%20%7C%20net9.0-informational)

</div>

<div align="center">

An easy-to-use Serilog sink that enables logging to SignalR in ASP.NET Core applications.

</div>

## Installation

    dotnet add package Serilog.Sinks.AspNetCore.App.SignalR

## Quick Start

### Default hub

The simplest setup. A hub is provided out of the box — no custom hub class required. Log events are streamed to clients by calling a named method (e.g. `ReceiveEvent`) on all connected clients.

```csharp
builder.Services.AddDefaultSerilogHub();
builder.Services.AddSerilog((serviceProvider, loggerConfiguration) =>
    loggerConfiguration.WriteTo.SignalR(serviceProvider, "ReceiveEvent"));

app.MapDefaultSerilogHub("/logs");
```

### Typed hub

Use when you need full control over how events are sent. Targets a hub you define and accepts a delegate that gives you direct access to the hub context, formatted message, and raw `LogEvent`.

```csharp
builder.Services.AddSerilogHub<MyHub>();
builder.Services.AddSerilog((serviceProvider, loggerConfiguration) =>
    loggerConfiguration.WriteTo.SignalR<MyHub>(serviceProvider,
        (context, message, logEvent) =>
            context.Clients.All.SendAsync("ReceiveEvent", message, logEvent)));

app.MapHub<MyHub>("/logs");
```

## Documentation

- [Full documentation](./src/README.md)
- [Samples](./samples/)
