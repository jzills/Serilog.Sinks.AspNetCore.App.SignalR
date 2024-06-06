using Microsoft.AspNetCore.SignalR;

namespace Serilog.Sinks.AspNetCore.App.SignalR;

/// <summary>
/// A <c>class</c> representing a <c>LazyHub&lt;THub&gt;</c> where
/// <c>THub</c> is a SignalR <c>Hub</c>.
/// </summary>
/// <typeparam name="THub">A SignalR <c>Hub</c>.</typeparam>
public class LazyHub<THub> where THub : Hub
{
    /// <summary>
    /// A lazily initialized <c>IHubContext</c>.
    /// </summary>
    private readonly Lazy<IHubContext<THub>> _context;

    /// <summary>
    /// Creates an instance of <c>LazyHub</c>.
    /// </summary>
    /// <param name="context">A lazily initialized <c>IHubContext</c>.</param>
    /// <returns>A <c>LazyHub</c> instance.</returns>
    public LazyHub(Lazy<IHubContext<THub>> context) => _context = context;
    
    /// <summary>
    /// An <c>IHubContext</c>.
    /// </summary>
    public IHubContext<THub> Context => _context.Value;
}