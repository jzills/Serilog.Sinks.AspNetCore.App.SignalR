using Microsoft.AspNetCore.SignalR;
using Serilog.Core;
using Serilog.Events;

namespace Serilog.Sinks.SignalR;

public class SignalRSink<THub> : ILogEventSink where THub : Hub
{
    private readonly SerilogHubWrapper<THub> _hubWrapper;
    private readonly string _hubMethod;
    private readonly IFormatProvider? _formatProvider;

    public SignalRSink(
        SerilogHubWrapper<THub> hubWrapper,
        string hubMethod,
        IFormatProvider? formatProvider
    )
    {
        _hubWrapper = hubWrapper;
        _hubMethod = hubMethod;
        _formatProvider = formatProvider;
    }

    public void Emit(LogEvent logEvent)
    {
        var message = logEvent.RenderMessage(_formatProvider);
        _hubWrapper.Context.Clients.All.SendAsync(_hubMethod, message).Wait();
    }
}
