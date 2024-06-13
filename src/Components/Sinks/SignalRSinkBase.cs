using Microsoft.AspNetCore.SignalR;

namespace Serilog.Sinks.AspNetCore.App.SignalR;

/// <summary>
/// A <c>class</c> representing a <c>SignalRSinkBase&lt;THub&gt;</c> where
/// <c>THub</c> is a SignalR <c>Hub</c>.
/// </summary>
/// <typeparam name="THub"><c>THub</c> is a SignalR <c>Hub</c>.</typeparam>
public abstract class SignalRSinkBase<THub> where THub : Hub
{
    /// <summary>
    /// A <c>LazyHub&lt;THub&gt;</c> instance.
    /// </summary>
    protected readonly LazyHub<THub> Hub;

    /// <summary>
    /// An <c>IFormatProvider</c> implementation.
    /// </summary>
    protected readonly IFormatProvider? FormatProvider;

    /// <summary>
    /// Creates an instance of <c>SignalRSinkBase</c>.
    ///     <para>
    ///         The hub parameter requires a <c>LazyHub&lt;THub&gt;</c> to
    ///         prevent circular dependencies during logger initialization.
    ///     </para>
    /// </summary>
    /// <param name="hub">A <c>LazyHub&lt;THub&gt;</c> instance.</param>
    /// <param name="formatProvider">An <c>IFormatProvider</c> implementation.</param>
    /// <returns>An instance of <c>SignalRSinkBase</c>.</returns>
    public SignalRSinkBase(LazyHub<THub> hub, IFormatProvider? formatProvider)
    {
        Hub = hub;
        FormatProvider = formatProvider;
    }
}