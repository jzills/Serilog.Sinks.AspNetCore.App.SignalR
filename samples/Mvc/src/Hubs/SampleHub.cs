using Microsoft.AspNetCore.SignalR;

namespace Web.Hubs;

public class SampleHub : Hub
{
    public async Task SendMessage(string message)
    {
        await Clients.All.SendAsync("ReceiveMessage", message);
    }

    public override Task OnConnectedAsync()
    {
        return base.OnConnectedAsync();
    }
}