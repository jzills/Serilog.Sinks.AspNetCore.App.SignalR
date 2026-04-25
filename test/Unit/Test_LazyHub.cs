using Microsoft.AspNetCore.SignalR;
using Moq;
using Serilog.Sinks.AspNetCore.App.SignalR;

namespace Unit;

[TestFixture]
public class Test_LazyHub
{
    [Test]
    public void LazyHub_2_Context_Returns_Value_From_Lazy()
    {
        // Arrange
        var mockContext = new Mock<IHubContext<TestTypedHub, ITestHubImplementation>>();
        var lazy = new Lazy<IHubContext<TestTypedHub, ITestHubImplementation>>(() => mockContext.Object);
        var lazyHub = new LazyHub<TestTypedHub, ITestHubImplementation>(lazy);

        // Act & Assert
        Assert.That(lazyHub.Context, Is.EqualTo(mockContext.Object));
    }
}
