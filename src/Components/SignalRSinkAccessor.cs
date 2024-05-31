using Microsoft.AspNetCore.SignalR;
using Serilog.Core;
using Serilog.Events;

namespace Serilog.Sinks.AspNetCore.SignalR;

public class SignalRSinkAccessor<THub> : SignalRSinkBase<THub>, ILogEventSink where THub : Hub
{
    private readonly Func<IHubContext<THub>, string, Task> _hubMethodAccessor;

    public SignalRSinkAccessor(
        LazyHub<THub> hub,
        Func<IHubContext<THub>, string, Task> hubMethodAccessor,
        IFormatProvider? formatProvider
    ) : base(hub, formatProvider)
    {
        _hubMethodAccessor = hubMethodAccessor;
    }

    public void Emit(LogEvent logEvent)
    {
        var message = logEvent.RenderMessage(FormatProvider);
        _hubMethodAccessor(Hub.Context, message).Wait();
    }
}