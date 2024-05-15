using Microsoft.AspNetCore.SignalR;

namespace Serilog.Sinks.SignalR;

public class SerilogHubWrapper<THub> where THub : Hub
{
    public readonly Lazy<IHubContext<THub>> _hubContext;

    public SerilogHubWrapper(Lazy<IHubContext<THub>> hubContext)
    {
        _hubContext = hubContext;
    }
    
    public IHubContext<THub> Context => _hubContext.Value;
}