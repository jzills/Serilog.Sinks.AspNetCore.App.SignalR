using Microsoft.AspNetCore.SignalR;

namespace Serilog.Sinks.AspNetCore.App.SignalR;

public class LazyHub<THub> where THub : Hub
{
    private readonly Lazy<IHubContext<THub>> _context;

    public LazyHub(Lazy<IHubContext<THub>> context) => _context = context;
    
    public IHubContext<THub> Context => _context.Value;
}