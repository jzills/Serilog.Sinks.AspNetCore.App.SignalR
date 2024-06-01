using Microsoft.AspNetCore.SignalR;

namespace Serilog.Sinks.AspNetCore.App.SignalR;

public class LazyHub<THub, TImplementation> 
    where THub : Hub<TImplementation>
    where TImplementation : class
{
    private readonly Lazy<IHubContext<THub, TImplementation>> _context;

    public LazyHub(Lazy<IHubContext<THub, TImplementation>> context) => _context = context;
    
    public IHubContext<THub, TImplementation> Context => _context.Value;
}