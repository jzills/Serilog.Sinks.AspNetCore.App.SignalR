using Serilog.Configuration;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;

namespace Serilog.Sinks.SignalR.Extensions
{
    public static class SignalRSinkExtensions
    {
        public static LoggerConfiguration SignalR<THub>(
                this LoggerSinkConfiguration loggerConfiguration,
                IServiceProvider serviceProvider,
                string hubMethod,
                IFormatProvider? formatProvider = null
        ) where THub : Hub
        {
            var hubWrapper = serviceProvider.GetRequiredService<SerilogHubWrapper<THub>>();
            return loggerConfiguration.Sink(
                new SignalRSink<THub>(hubWrapper, hubMethod, formatProvider));
        }
    }
}