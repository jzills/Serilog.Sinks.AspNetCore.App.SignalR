using Microsoft.AspNetCore.SignalR;

namespace Serilog.Sinks.AspNetCore.App.SignalR;

public class SignalRSinkBase<THub> where THub : Hub
{
    protected readonly LazyHub<THub> Hub;
    protected readonly IFormatProvider? FormatProvider;

    public SignalRSinkBase(LazyHub<THub> hub, IFormatProvider? formatProvider)
    {
        Hub = hub;
        FormatProvider = formatProvider;
    }
}