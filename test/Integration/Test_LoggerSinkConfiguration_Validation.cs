using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Sinks.AspNetCore.App.SignalR.Extensions;

namespace Integration;

[TestFixture]
public class Test_LoggerSinkConfiguration_Validation
{
    private IServiceProvider _serviceProvider = null!;

    [SetUp]
    public void SetUp()
    {
        _serviceProvider = new ServiceCollection()
            .AddDefaultSerilogHub()
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

    private static IConfiguration BuildConfig(string json)
    {
        var stream = new System.IO.MemoryStream(System.Text.Encoding.UTF8.GetBytes(json));
        return new ConfigurationBuilder().AddJsonStream(stream).Build();
    }
}
