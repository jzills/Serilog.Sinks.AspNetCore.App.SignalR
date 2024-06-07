# Mvc Sample

For information on configuring the `Program.cs`, visit [here](../../src/README.md).

## Usage

This sample configures a client connection to the registered SignalR `Hub`. This code can be found [here](./src/Views/Home/Index.cshtml).

    const hub = new signalR.HubConnectionBuilder()
        .withUrl("https://localhost:7110/sample")
        .configureLogging(signalR.LogLevel.Debug)
        .build();

The client `Hub` connection registers the `LogUser` event and appends messages sent by the SignalR `Hub`. The `Hub` is essentially a proxy that the Serilog sink communicates through.

    hub.on("LogUser", (message, name, email) => {
        ...
    });

Finally, the connection is made.

    hub.start();

When the sample project has loaded in the browser, duplicate the tab to watch events being logged in the first tab.