using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Serilog.Sinks.AspNetCore.App.SignalR;
using Serilog.Sinks.AspNetCore.App.SignalR.Extensions;

namespace Unit;

[TestFixture]
public class Test_IServiceCollectionExtensions
{
    [Test]
    public void AddDefaultSerilogHub_Registers_LazyHub_As_Singleton()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddDefaultSerilogHub();

        // Assert
        Assert.That(services.Any(service =>
            service.ServiceType == typeof(LazyHub<DefaultSerilogHub>) &&
            service.Lifetime == ServiceLifetime.Singleton), Is.True);
    }

    [Test]
    public void AddSerilogHub_Registers_LazyHubContext_As_Singleton()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddSerilogHub<TestHub>();

        // Assert
        Assert.That(services.Any(service =>
            service.ServiceType == typeof(Lazy<IHubContext<TestHub>>) &&
            service.Lifetime == ServiceLifetime.Singleton), Is.True);
    }

    [Test]
    public void AddSerilogHub_Registers_LazyHub_As_Singleton()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddSerilogHub<TestHub>();

        // Assert
        Assert.That(services.Any(service =>
            service.ServiceType == typeof(LazyHub<TestHub>) &&
            service.Lifetime == ServiceLifetime.Singleton), Is.True);
    }

    [Test]
    public void AddSerilogHub_THub_TImplementation_Registers_LazyHubContext_As_Singleton()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddSerilogHub<TestTypedHub, ITestHubImplementation>();

        // Assert
        Assert.That(services.Any(service =>
            service.ServiceType == typeof(Lazy<IHubContext<TestTypedHub, ITestHubImplementation>>) &&
            service.Lifetime == ServiceLifetime.Singleton), Is.True);
    }

    [Test]
    public void AddSerilogHub_THub_TImplementation_Registers_LazyHub_As_Singleton()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddSerilogHub<TestTypedHub, ITestHubImplementation>();

        // Assert
        Assert.That(services.Any(service =>
            service.ServiceType == typeof(LazyHub<TestTypedHub>) &&
            service.Lifetime == ServiceLifetime.Singleton), Is.True);
    }
}
