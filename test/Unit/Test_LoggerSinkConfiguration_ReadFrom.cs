using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Sinks.AspNetCore.App.SignalR.Extensions;

namespace Unit;

[TestFixture]
public class Test_LoggerSinkConfiguration_ReadFrom
{
    [Test]
    public void SignalR_ReadFrom_Configuration_Creates_Logger()
    {
        // Arrange
        var serviceProvider = new ServiceCollection()
            .AddDefaultSerilogHub()
            .BuildServiceProvider();

        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.test.json")
            .Build();

        // Act
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .WriteTo.SignalR(serviceProvider, configuration)
            .CreateLogger();

        // Assert
        Assert.That(Log.Logger, Is.Not.Null);
    }
}