using trackingAPI.Controllers;
using trackingAPI.Data;
using trackingAPI.Helpers;
using Microsoft.EntityFrameworkCore;
using trackingAPI.Models;

namespace trackingAPI.BackgroundHelpers;

public class LeagueBackgroundTask
{
    private readonly IServiceProvider _services;
    public LeagueBackgroundTask(IServiceProvider services)
    {
        _services = services;
    }
    public async Task CreateNewMatchesFromLeague(League league)
    {
        using (var scope = _services.CreateScope())
        {
            var _context =
                scope.ServiceProvider
                    .GetRequiredService<DatabaseContext>();

            Console.WriteLine("**************whileloop CreateNewMatchesFromLeague");
            MatchController matchController = new(_context);
            TeamPicker teamPicker = new();
            await matchController.Create(teamPicker);
            await _context.SaveChangesAsync();

        }
    }
}

