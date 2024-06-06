using Microsoft.AspNetCore.SignalR;

namespace Serilog.Sinks.AspNetCore.App.SignalR;

/// <summary>
/// A <c>class</c> representing a <c>LazyHub&lt;THub&gt;</c> where
/// <c>THub</c> is a SignalR <c>Hub&lt;TImplementation&gt;</c>.
/// </summary>
/// <typeparam name="THub">A SignalR <c>Hub&lt;TImplementation&gt;</c>.</typeparam>
/// <typeparam name="TImplementation">A strongly typed SignalR <c>Hub</c> implementation.</typeparam>
public class LazyHub<THub, TImplementation> 
    where THub : Hub<TImplementation>
    where TImplementation : class
{
    /// <summary>
    /// A lazily initialized <c>IHubContext</c>.
    /// </summary>
    private readonly Lazy<IHubContext<THub, TImplementation>> _context;

    /// <summary>
    /// Creates an instance of <c>LazyHub</c>.
    /// </summary>
    /// <param name="context">A lazily initialized <c>IHubContext</c>.</param>
    /// <returns>A <c>LazyHub</c> instance.</returns>
    public LazyHub(Lazy<IHubContext<THub, TImplementation>> context) => _context = context;
    
    /// <summary>
    /// An <c>IHubContext</c>.
    /// </summary>
    public IHubContext<THub, TImplementation> Context => _context.Value;
}