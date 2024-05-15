using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;

namespace Serilog.Sinks.SignalR;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSerilogHub<THub>(this IServiceCollection services) where THub : Hub
    {
        return services
            .AddSingleton(serviceProvider => new Lazy<IHubContext<THub>>(() => serviceProvider.GetRequiredService<IHubContext<THub>>()))
            .AddSingleton<SerilogHubWrapper<THub>>();
    } 
}