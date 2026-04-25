using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.AspNetCore.App.SignalR;
using Serilog.Sinks.AspNetCore.App.SignalR.Extensions;

namespace Unit;

[TestFixture]
public class Test_LoggerSinkConfigurationExtensions
{
    private IServiceProvider _serviceProvider = null!;

    [SetUp]
    public void SetUp()
    {
        _serviceProvider = new ServiceCollection()
            .AddDefaultSerilogHub()
            .AddSerilogHub<TestHub>()
            .BuildServiceProvider();
    }

    [Test]
    public void SignalR_Configuration_Throws_When_SignalR_Section_Missing()
    {
        // Arrange
        var config = BuildConfig(@"{""Serilog"":{""WriteTo"":[{""Name"":""Console""}]}}");

        // Act / Assert
        Assert.Throws<ArgumentException>(() =>
            new LoggerConfiguration().WriteTo.SignalR(_serviceProvider, config));
    }

    [Test]
    public void SignalR_Configuration_Throws_When_HubMethod_Missing()
    {
        // Arrange
        var config = BuildConfig(@"{""Serilog"":{""WriteTo"":[{""Name"":""SignalR"",""Args"":{}}]}}");

        // Act / Assert
        Assert.Throws<ArgumentException>(() =>
            new LoggerConfiguration().WriteTo.SignalR(_serviceProvider, config));
    }

    [Test]
    public void SignalR_ServiceProvider_HubMethod_Creates_Logger()
    {
        // Act
        var loggerConfig = new LoggerConfiguration()
            .WriteTo.SignalR(_serviceProvider, "ReceiveLog");

        // Assert
        Assert.That(loggerConfig, Is.Not.Null);
    }

    [Test]
    public void SignalR_THub_HubMethod_Creates_Logger()
    {
        // Act
        var loggerConfig = new LoggerConfiguration()
            .WriteTo.SignalR<TestHub>(_serviceProvider, "ReceiveLog");

        // Assert
        Assert.That(loggerConfig, Is.Not.Null);
    }

    [Test]
    public void SignalR_THub_Accessor_Creates_Logger()
    {
        // Arrange
        Func<IHubContext<TestHub>, string, LogEvent, Task> accessor =
            (_, _, _) => Task.CompletedTask;

        // Act
        var loggerConfig = new LoggerConfiguration()
            .WriteTo.SignalR<TestHub>(_serviceProvider, accessor);

        // Assert
        Assert.That(loggerConfig, Is.Not.Null);
    }

    [Test]
    public void SignalR_THub_MessageAccessor_Creates_Logger()
    {
        // Arrange
        Func<IHubContext<TestHub>, string, Task> accessor =
            (_, _) => Task.CompletedTask;

        // Act
        var loggerConfig = new LoggerConfiguration()
            .WriteTo.SignalR<TestHub>(_serviceProvider, accessor);

        // Assert
        Assert.That(loggerConfig, Is.Not.Null);
    }

    private static IConfiguration BuildConfig(string json)
    {
        var stream = new System.IO.MemoryStream(System.Text.Encoding.UTF8.GetBytes(json));
        return new ConfigurationBuilder().AddJsonStream(stream).Build();
    }
}
