# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Build and Test Commands

Build the library:
```sh
dotnet build src/Serilog.Sinks.AspNetCore.App.SignalR.csproj
```

Run all tests:
```sh
dotnet test
```

Run a specific test project:
```sh
dotnet test test/Unit/Unit.csproj
dotnet test test/Integration/Integration.csproj
```

Run a single test by name:
```sh
dotnet test test/Unit/Unit.csproj --filter "FullyQualifiedName~Test_ReadFrom_Configuration"
```

Pack the NuGet package:
```sh
dotnet pack src/Serilog.Sinks.AspNetCore.App.SignalR.csproj
```

The library targets `net8.0` and `net9.0`. Tests also multi-target both frameworks.

## Architecture

This is a Serilog sink that routes log events to SignalR hubs in ASP.NET Core applications.

### Core Problem: Circular Dependency

Serilog is initialized early in the DI pipeline, before `IHubContext<T>` is available. The `LazyHub<THub>` wrapper (`src/Components/LazyHubs/`) solves this by deferring resolution of `IHubContext` until the first log event fires. `AddSerilogHub<THub>()` registers both a `Lazy<IHubContext<THub>>` and a `LazyHub<THub>` as singletons — this is why `AddSerilogHub` must always be called before `AddSerilog`.

### Sink Variants

Three concrete sinks inherit from `SignalRSinkBase<THub>` (`src/Components/Sinks/`):

- **`SignalRSinkMethod<THub>`** — sends `(message, logEvent)` to a named hub method string (e.g. `"ReceiveEvent"`)
- **`SignalRSinkAccessor<THub>`** — takes a `Func<IHubContext<THub>, string, LogEvent, Task>` delegate for full control over message + log event
- **`SignalRSinkMessageAccessor<THub>`** — takes a `Func<IHubContext<THub>, string, Task>` delegate for message-only control

All three call `.Wait()` on the async hub send inside `Emit()`, making the sink synchronous as Serilog requires.

### Extension Methods

- `IServiceCollectionExtensions` — `AddSerilogHub<THub>()`, `AddDefaultSerilogHub()`, typed overload for `Hub<TImplementation>`
- `LoggerSinkConfigurationExtensions` — `WriteTo.SignalR(...)` overloads selecting the appropriate sink variant
- `WebApplicationExtensions` / `IEndpointRouteBuilderExtensions` — `MapDefaultSerilogHub(route)` for routing the built-in hub

### Default Hub

`DefaultSerilogHub` is an empty `Hub` subclass used when no custom hub is needed. Registered via `AddDefaultSerilogHub()` and mapped via `MapDefaultSerilogHub(route)`.

### Configuration-Based Setup

The `WriteTo.SignalR(serviceProvider, IConfiguration)` overload reads `Serilog:WriteTo[Name=SignalR]:Args:HubMethod` from config. Only `HubMethod` is supported as a config argument; it always uses `DefaultSerilogHub`.

### Samples

`samples/` contains four standalone MVC apps demonstrating the different registration patterns:
- `Mvc` — user-defined hub with accessor delegate
- `MvcWithDefaultHub` — default hub with method name string
- `MvcLoggerConfiguration` — configuration-based setup
- `MvcWithCustomLogProperties` — Serilog enrichers with a custom hub
