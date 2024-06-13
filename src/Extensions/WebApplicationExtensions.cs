using Microsoft.AspNetCore.Builder;

namespace Serilog.Sinks.AspNetCore.App.SignalR.Extensions;

/// <summary>
/// A <c>class</c> representing extension methods for <c>WebApplication</c>.
/// </summary>
public static class WebApplicationExtensions
{
    /// <summary>
    /// Maps the <c>DefaultSerilogHub</c> to the specified pattern.
    /// </summary>
    /// <param name="app">An instance of <c>WebApplication</c>.</param>
    /// <param name="pattern">A <c>string</c> representing the route pattern.</param>
    /// <returns>An instance of <c>HubEndpointConventionBuilder</c> for chaining subsequent calls.</returns>
    public static HubEndpointConventionBuilder MapDefaultSerilogHub(
        this WebApplication app, 
        string pattern
    ) => app.MapHub<DefaultSerilogHub>(pattern);
}