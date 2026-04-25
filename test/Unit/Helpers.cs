using Microsoft.AspNetCore.SignalR;

namespace Unit;

public interface ITestHubImplementation { }

public class TestHub : Hub { }

public class TestTypedHub : Hub<ITestHubImplementation> { }
