using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace Serilog.Sinks.AspNetCore.App.SignalR.Extensions;

/// <summary>
/// Provides extension methods for <see cref="IEndpointRouteBuilder"/> to map SignalR hubs.
/// </summary>
public static class IEndpointRouteBuilderExtensions
{
    /// <summary>
    /// Maps the <see cref="DefaultSerilogHub"/> to a specified route.
    /// </summary>
    /// <param name="builder">The <see cref="IEndpointRouteBuilder"/> to configure.</param>
    /// <param name="route">The route to map the hub to.</param>
    /// <remarks>
    /// This method maps a SignalR hub to the given route, enabling real-time communication
    /// with clients through the <see cref="DefaultSerilogHub"/> hub.
    /// </remarks>
    public static void MapDefaultSerilogHub(
        this IEndpointRouteBuilder builder, 
        string route
    ) => builder.MapHub<DefaultSerilogHub>(route);
}