using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using trackingAPI.Models;

namespace trackingAPI.Hubs;

public class TestHub : Hub
{
    public static int TotalViews { get; set; } = 0;
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

    public async Task MatchUpdated(Gamematch gamematch)
    {
        await Clients.Client(this.Context.ConnectionId).SendAsync("matchUpdatedResponse", gamematch);
    }

    public async Task NewWindowLoaded()
    {
        TotalViews++;
        //send update to all clients that total views have been updated
        await Clients.All.SendAsync("updateTotalViews", TotalViews);
    }

    //public async Task<string> NewWindowLoaded(string name)
    //{
    //    TotalViews++;
    //    //send update to all clients that total views have been updated
    //    await Clients.All.SendAsync("updateTotalViews", TotalViews);
    //    return $"total views from {name} - {TotalViews}";
    //}
}
