using Microsoft.AspNetCore.SignalR;

namespace trackingAPI.Hubs;

public class TestHub : Hub
{
    public Task SendMessage(string user, string message)
    {
        return Clients.All.SendAsync("ReceiveMessage123", user, message);
    }
}
