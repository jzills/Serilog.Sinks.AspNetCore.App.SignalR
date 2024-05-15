using Microsoft.AspNetCore.SignalR;
using Serilog.Core;
using Serilog.Events;

namespace Serilog.Sinks.SignalR;

public class SignalRSinkMethod<THub> : SignalRSinkBase<THub>, ILogEventSink where THub : Hub
{
    private readonly string _hubMethod;

    public SignalRSinkMethod(
        LazyHub<THub> hub,
        string hubMethod,
        IFormatProvider? formatProvider
    ) : base(hub, formatProvider)
    {
        _hubMethod = hubMethod;
    }

    public void Emit(LogEvent logEvent)
    {
        var message = logEvent.RenderMessage(FormatProvider);
        Hub.Context.Clients.All.SendAsync(_hubMethod, message).Wait();
    }
}
