using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Sinks.AspNetCore.App.SignalR.Extensions;

namespace Unit;

[TestFixture]
public class Test_ReadFrom_Configuration
{
    [Test]
    public void Test()
    {
        var serviceProvider = new ServiceCollection()
            .AddDefaultSerilogHub()
            .BuildServiceProvider();

        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.test.json")
            .Build();

        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .WriteTo.SignalR(serviceProvider, configuration)
            .CreateLogger();

        Log.Information("This is a test log using the custom sink.");
    }
}