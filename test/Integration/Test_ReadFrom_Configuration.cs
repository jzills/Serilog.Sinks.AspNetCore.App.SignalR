using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Serilog.Sinks.AspNetCore.App.SignalR.Extensions;
using Serilog;

namespace Integration;

[TestFixture]
public class Test_ReadFrom_Configuration
{
    TestServer TestServer;
    HubConnection Connection;

    [SetUp]
    public void Setup()
    {
        var webHostBuilder = new WebHostBuilder()
            .ConfigureAppConfiguration((context, config) =>
            {
                config.AddJsonFile("appsettings.test.json");
            })
            .ConfigureServices(services =>
            {
                services.AddSignalR();
                services.AddDefaultSerilogHub();
                services.AddSerilog((serviceProvider, options) => 
                {
                    var config = serviceProvider.GetRequiredService<IConfiguration>();
                    options.ReadFrom.Configuration(config)
                        .WriteTo.SignalR(serviceProvider, config);
                });
            })
            .Configure(app =>
            {
                app.UseRouting();
                app.UseEndpoints(endpoints =>
                {
                    endpoints.MapDefaultSerilogHub("/hub");
                });
            });

        TestServer = new TestServer(webHostBuilder);
        Connection = new HubConnectionBuilder()
            .WithUrl(new Uri(TestServer.BaseAddress, "hub"), options =>
                options.HttpMessageHandlerFactory = _ => TestServer.CreateHandler())
            .Build();
    }

    [Test]
    public async Task Test_SignalR_Message()
    {
        var completionSource = new TaskCompletionSource<string>();

        Connection.On<string, object>("ReceiveLogEvent", (message, _) => 
            completionSource.TrySetResult(message));

        await Connection.StartAsync();

        var message = await completionSource.Task;

        Assert.That(message.StartsWith("Request starting"));
    }

    [TearDown]
    public async Task TearDown()
    {
        TestServer.Dispose();
        await Connection.StopAsync();
    }
}