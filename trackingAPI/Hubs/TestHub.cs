using Microsoft.AspNetCore.SignalR;

namespace trackingAPI.Hubs;

public class TestHub : Hub
{
    public async Task AskServer(string message)
    {
        string tempString;

        if (message == "hey")
        {
            tempString = "message was 'hey'";
        }
        else
        {
            tempString = "message was something else";
        }

        //await Clients.Client(this.Context.ConnectionId).SendAsync("askServerResponse", tempString);
        await Clients.Client(this.Context.ConnectionId).SendAsync("askServerResponse", tempString);
    }
}