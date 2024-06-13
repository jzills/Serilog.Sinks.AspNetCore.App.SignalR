using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Serilog.Configuration;
using Serilog.Events;

namespace Serilog.Sinks.AspNetCore.App.SignalR.Extensions;

/// <summary>
/// A <c>class</c> representing extension methods for <c>LoggerSinkConfiguration</c>.
/// </summary>
public static class LoggerSinkConfigurationExtensions
{
    /// <summary>
    /// Registers an instance of <c>DefaultSerilogHub</c> to handle Serilog communication with SignalR.
    /// </summary>
    /// <remarks>
    /// The <c>DefaultSerilogHub</c> must also be registered through a call to <c>AddDefaultSerilogHub</c> on the <c>IServiceCollection</c>.
    /// </remarks>
    /// <param name="loggerConfiguration">This instance of <c>LoggerSinkConfiguration</c>.</param>
    /// <param name="serviceProvider">An <c>IServiceProvider</c>.</param>
    /// <param name="hubMethod">A <c>string</c> representing the <c>Hub</c> method to push <c>LogEvent</c> objects to.</param>
    /// <param name="formatProvider">An <c>IFormatProvider</c>.</param>
    /// <returns>A <c>LoggerConfiguration</c> for chaining subsequent calls.</returns>
    public static LoggerConfiguration SignalR(
        this LoggerSinkConfiguration loggerConfiguration,
        IServiceProvider serviceProvider,
        string hubMethod,
        IFormatProvider? formatProvider = null
    ) =>
        loggerConfiguration.Sink(
            new SignalRSinkMethod<DefaultSerilogHub>(
                serviceProvider.GetRequiredService<LazyHub<DefaultSerilogHub>>(), 
                hubMethod, 
                formatProvider
            ));

    /// <summary>
    /// Registers <c>THub</c> as a <c>LazyHub</c> which is a very simple wrapper around an <c>IHubContext</c>. 
    /// This is to prevent circular dependencies during logger initialization.
    ///     <para>
    ///         The specified SignalR <c>Hub</c> method is called when Serilog writes out events.
    ///     </para>
    ///     <para>
    ///         Usage examples can be found <see href="https://github.com/jzills/Serilog.Sinks.AspNetCore.App.SignalR/tree/main/samples/Mvc">here</see>, including complete code samples.<br />
    ///         More details about the <c>LazyHub</c> implementation can be found <see href="https://github.com/jzills/Serilog.Sinks.AspNetCore.App.SignalR/tree/main/src/Components/LazyHubs">here</see>.
    ///     </para>
    /// </summary>
    /// <param name="loggerConfiguration">This instance of <c>LoggerSinkConfiguration</c>.</param>
    /// <param name="serviceProvider">An <c>IServiceProvider</c>.</param>
    /// <param name="hubMethod">A <c>string</c> representing the <c>Hub</c> method to push <c>LogEvent</c> objects to.</param>
    /// <param name="formatProvider">An <c>IFormatProvider</c>.</param>
    /// <typeparam name="THub">>A SignalR <c>Hub</c>.</typeparam>
    /// <returns>A <c>LoggerConfiguration</c> for chaining subsequent calls.</returns>
    public static LoggerConfiguration SignalR<THub>(
        this LoggerSinkConfiguration loggerConfiguration,
        IServiceProvider serviceProvider,
        string hubMethod,
        IFormatProvider? formatProvider = null
    ) where THub : Hub =>
        loggerConfiguration.Sink(
            new SignalRSinkMethod<THub>(
                serviceProvider.GetRequiredService<LazyHub<THub>>(), 
                hubMethod, 
                formatProvider
            ));

    /// <summary>
    /// Registers <c>THub</c> as a <c>LazyHub</c> which is a very simple wrapper around an <c>IHubContext</c>. 
    /// This is to prevent circular dependencies during logger initialization.
    ///     <para>
    ///         The specified SignalR <c>Hub</c> method is called when Serilog writes out events.
    ///     </para>
    ///     <para>
    ///         Usage examples can be found <see href="https://github.com/jzills/Serilog.Sinks.AspNetCore.App.SignalR/tree/main/samples/Mvc">here</see>, including complete code samples.<br />
    ///         More details about the <c>LazyHub</c> implementation can be found <see href="https://github.com/jzills/Serilog.Sinks.AspNetCore.App.SignalR/tree/main/src/Components/LazyHubs">here</see>.
    ///     </para>
    /// </summary>
    /// <param name="loggerConfiguration">This instance of <c>LoggerSinkConfiguration</c>.</param>
    /// <param name="serviceProvider">An <c>IServiceProvider</c>.</param>
    /// <param name="hubMethodAccessor">A <c>Func&lt;IHubContext&lt;THub&gt;, string, LogEvent, Task&gt;</c> used to access the SignalR <c>Hub</c> method to push log events to.</param>
    /// <param name="formatProvider">An <c>IFormatProvider</c>.</param>
    /// <typeparam name="THub">>A SignalR <c>Hub</c>.</typeparam>
    /// <returns>A <c>LoggerConfiguration</c> for chaining subsequent calls.</returns>
    public static LoggerConfiguration SignalR<THub>(
        this LoggerSinkConfiguration loggerConfiguration,
        IServiceProvider serviceProvider,
        Func<IHubContext<THub>, string, LogEvent, Task> hubMethodAccessor,
        IFormatProvider? formatProvider = null
    ) where THub : Hub => 
        loggerConfiguration.Sink(
            new SignalRSinkAccessor<THub>(
                serviceProvider.GetRequiredService<LazyHub<THub>>(), 
                hubMethodAccessor, 
                formatProvider
            ));

    /// <summary>
    /// Registers <c>THub</c> as a <c>LazyHub</c> which is a very simple wrapper around an <c>IHubContext</c>. 
    /// This is to prevent circular dependencies during logger initialization.
    ///     <para>
    ///         The specified SignalR <c>Hub</c> method is called when Serilog writes out events.
    ///     </para>
    ///     <para>
    ///         Usage examples can be found <see href="https://github.com/jzills/Serilog.Sinks.AspNetCore.App.SignalR/tree/main/samples/Mvc">here</see>, including complete code samples.<br />
    ///         More details about the <c>LazyHub</c> implementation can be found <see href="https://github.com/jzills/Serilog.Sinks.AspNetCore.App.SignalR/tree/main/src/Components/LazyHubs">here</see>.
    ///     </para>
    /// </summary>
    /// <param name="loggerConfiguration">This instance of <c>LoggerSinkConfiguration</c>.</param>
    /// <param name="serviceProvider">An <c>IServiceProvider</c>.</param>
    /// <param name="hubMethodAccessor">A <c>Func&lt;IHubContext&lt;THub&gt;, string, Task&gt;</c> used to access the SignalR <c>Hub</c> method to push log events to.</param>
    /// <param name="formatProvider">An <c>IFormatProvider</c>.</param>
    /// <typeparam name="THub">>A SignalR <c>Hub</c>.</typeparam>
    /// <returns>A <c>LoggerConfiguration</c> for chaining subsequent calls.</returns>
    public static LoggerConfiguration SignalR<THub>(
        this LoggerSinkConfiguration loggerConfiguration,
        IServiceProvider serviceProvider,
        Func<IHubContext<THub>, string, Task> hubMethodAccessor,
        IFormatProvider? formatProvider = null
    ) where THub : Hub => 
        loggerConfiguration.Sink(
            new SignalRSinkMessageAccessor<THub>(
                serviceProvider.GetRequiredService<LazyHub<THub>>(), 
                hubMethodAccessor, 
                formatProvider
            ));
}