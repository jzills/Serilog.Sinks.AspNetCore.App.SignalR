using Serilog.Configuration;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;

namespace Serilog.Sinks.SignalR.Extensions
{
    public static class LoggerSinkConfigurationExtensions
    {
        public static LoggerConfiguration SignalR<THub>(
            this LoggerSinkConfiguration loggerConfiguration,
            IServiceProvider serviceProvider,
            string hubMethod,
            IFormatProvider? formatProvider = null
        ) where THub : Hub =>
            loggerConfiguration.Sink(
                new SignalRSinkMethod<THub>(
                    serviceProvider.GetRequiredService<LazyHub<THub>>(), 
                    hubMethod, 
                    formatProvider
                ));

        public static LoggerConfiguration SignalR<THub>(
            this LoggerSinkConfiguration loggerConfiguration,
            IServiceProvider serviceProvider,
            Func<IHubContext<THub>, string, Task> hubMethodAccessor,
            IFormatProvider? formatProvider = null
        ) where THub : Hub => 
            loggerConfiguration.Sink(
                new SignalRSinkAccessor<THub>(
                    serviceProvider.GetRequiredService<LazyHub<THub>>(), 
                    hubMethodAccessor, 
                    formatProvider
                ));
    }
}