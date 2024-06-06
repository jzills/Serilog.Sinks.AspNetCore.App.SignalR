using Microsoft.AspNetCore.SignalR;
using Serilog.Core;
using Serilog.Events;

namespace Serilog.Sinks.AspNetCore.App.SignalR;

/// <summary>
/// A <c>class</c> representing a <c>SignalRSinkAccessor&lt;THub&gt;</c> where
/// <c>THub</c> is a SignalR <c>Hub</c>.
/// </summary>
/// <typeparam name="THub">
/// <c>THub</c> is a SignalR <c>Hub</c>.
/// </typeparam>
public class SignalRSinkAccessor<THub> : SignalRSinkBase<THub>, ILogEventSink where THub : Hub
{
    private readonly Func<IHubContext<THub>, string, LogEvent, Task> _hubMethodAccessor;

    /// <summary>
    /// Creates an instance of <c>SignalRSinkAccessor</c>.
    ///     <para>
    ///         The hub parameter requires a <c>LazyHub&lt;THub&gt;</c> to
    ///         prevent circular dependencies during logger initialization.
    ///     </para>
    /// </summary>
    /// <param name="hub">A <c>LazyHub&lt;THub&gt;</c>.</param>
    /// <param name="hubMethodAccessor">A <c>Func&lt;IHubContext&lt;THub&gt;, string, LogEvent, Task&gt;</c> used to access the SignalR <c>Hub</c> method to push log events to.</param>
    /// <param name="formatProvider">An <c>IFormatProvider</c>.</param>
    /// <returns>An instance of <c>SignalRSinkAccessor</c>.</returns>
    public SignalRSinkAccessor(
        LazyHub<THub> hub,
        Func<IHubContext<THub>, string, LogEvent, Task> hubMethodAccessor,
        IFormatProvider? formatProvider
    ) : base(hub, formatProvider)
    {
        _hubMethodAccessor = hubMethodAccessor;
    }

    /// <summary>
    /// Emits the specified <c>LogEvent</c> as a formatted message
    /// to the <c>Hub</c> specified during initialization.
    /// </summary>
    /// <param name="logEvent">A <c>LogEvent</c>.</param>
    public void Emit(LogEvent logEvent)
    {
        var message = logEvent.RenderMessage(FormatProvider);
        _hubMethodAccessor(Hub.Context, message, logEvent).Wait();
    }
}