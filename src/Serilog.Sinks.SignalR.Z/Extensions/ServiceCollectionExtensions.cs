using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;

namespace Serilog.Sinks.SignalR;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSerilogHub<THub>(
        this IServiceCollection services
    ) where THub : Hub =>
        services
            .AddSingleton(serviceProvider => 
                new Lazy<IHubContext<THub>>(() => 
                    serviceProvider.GetRequiredService<IHubContext<THub>>()))
            .AddSingleton<LazyHub<THub>>();

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