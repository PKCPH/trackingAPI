using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using trackingAPI.Models;

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

    //public async Task RunLiveMatchFrontEnd(string message)
    //{

    //    await Clients.Client(this.Context.ConnectionId).SendAsync(message, tempstring);
    //}
}

public class TestHubHelper
{
    //get match info in this class and pass as string to testHub to return;

    //public string GetMatchInRealTime(Guid gamematchId)
    //{

    //}
}