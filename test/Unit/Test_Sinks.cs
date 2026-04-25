using Microsoft.AspNetCore.SignalR;
using Moq;
using Serilog.Events;
using Serilog.Parsing;
using Serilog.Sinks.AspNetCore.App.SignalR;

namespace Unit;

[TestFixture]
public class Test_Sinks
{
    private Mock<IHubContext<TestHub>> _mockHubContext = null!;
    private LazyHub<TestHub> _lazyHub = null!;
    private LogEvent _logEvent = null!;

    [SetUp]
    public void SetUp()
    {
        var mockClientProxy = new Mock<IClientProxy>();
        mockClientProxy
            .Setup(clientProxy => clientProxy.SendCoreAsync(
                It.IsAny<string>(),
                It.IsAny<object?[]>(),
                It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var mockClients = new Mock<IHubClients>();
        mockClients.Setup(clients => clients.All).Returns(mockClientProxy.Object);

        _mockHubContext = new Mock<IHubContext<TestHub>>();
        _mockHubContext.Setup(hubContext => hubContext.Clients).Returns(mockClients.Object);

        _lazyHub = new LazyHub<TestHub>(
            new Lazy<IHubContext<TestHub>>(() => _mockHubContext.Object));

        var template = new MessageTemplateParser().Parse("Test message");
        _logEvent = new LogEvent(
            DateTimeOffset.UtcNow,
            LogEventLevel.Information,
            null,
            template,
            []);
    }

    [Test]
    public void SignalRSinkMethod_Emit_Sends_Message_And_LogEvent_To_Hub()
    {
        // Arrange
        var sink = new SignalRSinkMethod<TestHub>(_lazyHub, "ReceiveLog", null);

        // Act
        sink.Emit(_logEvent);

        // Assert
        _mockHubContext.Verify(hubContext => hubContext.Clients, Times.Once);
    }

    [Test]
    public void SignalRSinkAccessor_Emit_Invokes_Accessor_With_Context_Message_And_LogEvent()
    {
        // Arrange
        IHubContext<TestHub>? capturedContext = null;
        string? capturedMessage = null;
        LogEvent? capturedEvent = null;

        var sink = new SignalRSinkAccessor<TestHub>(
            _lazyHub,
            (context, message, logEvent) =>
            {
                capturedContext = context;
                capturedMessage = message;
                capturedEvent = logEvent;
                return Task.CompletedTask;
            },
            null);

        // Act
        sink.Emit(_logEvent);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(capturedContext, Is.EqualTo(_mockHubContext.Object));
            Assert.That(capturedMessage, Is.EqualTo("Test message"));
            Assert.That(capturedEvent, Is.EqualTo(_logEvent));
        });
    }

    [Test]
    public void SignalRSinkMessageAccessor_Emit_Invokes_Accessor_With_Context_And_Message()
    {
        // Arrange
        IHubContext<TestHub>? capturedContext = null;
        string? capturedMessage = null;

        var sink = new SignalRSinkMessageAccessor<TestHub>(
            _lazyHub,
            (context, message) =>
            {
                capturedContext = context;
                capturedMessage = message;
                return Task.CompletedTask;
            },
            null);

        // Act
        sink.Emit(_logEvent);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(capturedContext, Is.EqualTo(_mockHubContext.Object));
            Assert.That(capturedMessage, Is.EqualTo("Test message"));
        });
    }
}
