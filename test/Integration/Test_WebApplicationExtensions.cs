using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Sinks.AspNetCore.App.SignalR.Extensions;

namespace Integration;

[TestFixture]
public class Test_WebApplicationExtensions
{
    private WebApplication _app = null!;
    private HubConnection _connection = null!;

    [SetUp]
    public async Task SetUp()
    {
        var builder = WebApplication.CreateBuilder();
        builder.WebHost.UseTestServer();
        builder.Services.AddSignalR();
        builder.Services.AddDefaultSerilogHub();
        builder.Services.AddSerilog((serviceProvider, loggerConfiguration) =>
            loggerConfiguration.WriteTo.SignalR(serviceProvider, "ReceiveLogEvent"));

        _app = builder.Build();
        _app.MapDefaultSerilogHub("/hub");
        await _app.StartAsync();

        var testServer = _app.GetTestServer();
        _connection = new HubConnectionBuilder()
            .WithUrl(new Uri(testServer.BaseAddress, "hub"), options =>
                options.HttpMessageHandlerFactory = _ => testServer.CreateHandler())
            .Build();
    }

    [Test]
    public async Task MapDefaultSerilogHub_Routes_Hub_And_Delivers_Messages()
    {
        // Arrange
        var completionSource = new TaskCompletionSource<string>();
        _connection.On<string, object>("ReceiveLogEvent", (message, _) =>
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
        await _connection.StopAsync();
        await _app.StopAsync();
    }
}
