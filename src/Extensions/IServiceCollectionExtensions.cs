using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;

namespace Serilog.Sinks.AspNetCore.App.SignalR.Extensions;

/// <summary>
/// A class representing extension methods for <c>IServiceCollection</c>.
/// </summary>
public static class IServiceCollectionExtensions
{
    /// <summary>
    /// Registers <c>THub</c> as a <c>LazyHub</c> which is a very simple wrapper around an <c>IHubContext</c>. 
    /// This is to prevent circular dependencies during logger initialization.
    ///     <para>
    ///         Usage examples can be found <see href="https://github.com/jzills/Serilog.Sinks.AspNetCore.App.SignalR/tree/main/samples/Mvc">here</see>, including complete code samples.<br />
    ///         More details about the <c>LazyHub</c> implementation can be found <see href="https://github.com/jzills/Serilog.Sinks.AspNetCore.App.SignalR/tree/main/src/Components/LazyHubs">here</see>.
    ///     </para>
    /// </summary>
    /// <param name="services">A <c>IServiceCollection</c>.</param>
    /// <typeparam name="THub">A SignalR <c>Hub</c>.</typeparam>
    /// <returns>An <c>IServiceCollection</c>.</returns>
    public static IServiceCollection AddSerilogHub<THub>(
        this IServiceCollection services
    ) where THub : Hub =>
        services
            .AddSingleton(serviceProvider => 
                new Lazy<IHubContext<THub>>(() => 
                    serviceProvider.GetRequiredService<IHubContext<THub>>()))
            .AddSingleton<LazyHub<THub>>();

    /// <summary>
    /// Registers <c>THub</c> as a <c>LazyHub</c> which is a very simple wrapper around an <c>IHubContext</c>. 
    /// This is to prevent circular dependencies during logger initialization.
    ///     <para>
    ///         Usage examples can be found <see href="https://github.com/jzills/Serilog.Sinks.AspNetCore.App.SignalR/tree/main/samples/Mvc">here</see>, including complete code samples.<br />
    ///         More details about the <c>LazyHub</c> implementation can be found <see href="https://github.com/jzills/Serilog.Sinks.AspNetCore.App.SignalR/tree/main/src/Components/LazyHubs">here</see>.
    ///     </para>
    /// </summary>
    /// <param name="services">A <c>IServiceCollection</c>.</param>
    /// <typeparam name="THub">A SignalR <c>Hub</c>.</typeparam>
    /// <typeparam name="TImplementation">A strongly typed SignalR <c>Hub</c> implementation.</typeparam>
    /// <returns>An <c>IServiceCollection</c>.</returns>
    public static IServiceCollection AddSerilogHub<THub, TImplementation>(
        this IServiceCollection services
    ) where THub : Hub<TImplementation>
      where TImplementation : class =>
        services
            .AddSingleton(serviceProvider => 
                new Lazy<IHubContext<THub, TImplementation>>(() => 
                    serviceProvider.GetRequiredService<IHubContext<THub, TImplementation>>()))
            .AddSingleton<LazyHub<THub>>();
}