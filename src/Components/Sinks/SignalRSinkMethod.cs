using Microsoft.AspNetCore.SignalR;
using Serilog.Core;
using Serilog.Events;

namespace Serilog.Sinks.AspNetCore.App.SignalR;

/// <summary>
/// A <c>class</c> representing a <c>SignalRSinkMethod&lt;THub&gt;</c> where
/// <c>THub</c> is a SignalR <c>Hub</c>.
/// </summary>
/// <typeparam name="THub">
/// <c>THub</c> is a SignalR <c>Hub</c>.
/// </typeparam>
public class SignalRSinkMethod<THub> : SignalRSinkBase<THub>, ILogEventSink where THub : Hub
{
    /// <summary>
    /// A <c>string</c> representing the name of the SignalR <c>Hub</c> method.
    /// </summary>
    private readonly string _hubMethod;

    /// <summary>
    /// Creates an instance of <c>SignalRSinkMethod</c>.
    ///     <para>
    ///         The hub parameter requires a <c>LazyHub&lt;THub&gt;</c> to
    ///         prevent circular dependencies during logger initialization.
    ///     </para>
    /// </summary>
    /// <param name="hub">A <c>LazyHub&lt;THub&gt;</c>.</param>
    /// <param name="hubMethod">A <c>string</c> representing the <c>Hub</c> method to push <c>LogEvent</c> objects to.</param>
    /// <param name="formatProvider">An <c>IFormatProvider</c>.</param>
    /// <returns>An instance of <c>SignalRSinkMethod</c>.</returns>
    public SignalRSinkMethod(
        LazyHub<THub> hub,
        string hubMethod,
        IFormatProvider? formatProvider
    ) : base(hub, formatProvider)
    {
        _hubMethod = hubMethod;
    }

    /// <summary>
    /// Emits the specified <c>LogEvent</c> as a formatted message
    /// to the <c>Hub</c> specified during initialization.
    /// </summary>
    /// <param name="logEvent">A <c>LogEvent</c>.</param>
    public void Emit(LogEvent logEvent)
    {
        var message = logEvent.RenderMessage(FormatProvider);
        Hub.Context.Clients.All.SendAsync(_hubMethod, message, logEvent).Wait();
    }
}