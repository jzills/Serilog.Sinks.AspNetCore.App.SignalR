using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Sinks.AspNetCore.App.SignalR.Extensions;

namespace Integration;

[TestFixture]
public class Test_SignalRSink_MessageAccessor
{
    private TestServer _testServer = null!;
    private HubConnection _connection = null!;

    [SetUp]
    public void SetUp()
    {
        var webHostBuilder = new WebHostBuilder()
            .ConfigureServices(services =>
            {
                services.AddSignalR();
                services.AddSerilogHub<TestSerilogHub>();
                services.AddSerilog((serviceProvider, loggerConfiguration) =>
                    loggerConfiguration.WriteTo.SignalR<TestSerilogHub>(
                        serviceProvider,
                        (context, message) => context.Clients.All.SendCoreAsync("Receive", [message])));
            })
            .Configure(app =>
            {
                app.UseRouting();
                app.UseEndpoints(endpoints => endpoints.MapHub<TestSerilogHub>("/hub"));
            });

        _testServer = new TestServer(webHostBuilder);
        _connection = new HubConnectionBuilder()
            .WithUrl(new Uri(_testServer.BaseAddress, "hub"), options =>
                options.HttpMessageHandlerFactory = _ => _testServer.CreateHandler())
            .Build();
    }

    [Test]
    public async Task SignalR_THub_MessageAccessor_Emit_Sends_Message_To_Hub()
    {
        // Arrange
        var completionSource = new TaskCompletionSource<string>();
        _connection.On<string>("Receive", message =>
            completionSource.TrySetResult(message));

        // Act
        await _connection.StartAsync();
        var message = await completionSource.Task;

        // Assert
        Assert.That(message, Is.Not.Null.And.Not.Empty);
    }

    [TearDown]
    public async Task TearDown()
    {
        _testServer.Dispose();
        await _connection.StopAsync();
    }
}
