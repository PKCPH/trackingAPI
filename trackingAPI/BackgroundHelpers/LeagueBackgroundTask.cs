using trackingAPI.Controllers;
using trackingAPI.Data;
using trackingAPI.Helpers;
using Microsoft.EntityFrameworkCore;
namespace trackingAPI.BackgroundHelpers;

public class LeagueBackgroundTask
{
    private readonly IServiceProvider _services;
    public LeagueBackgroundTask(IServiceProvider services)
    {
        _services = services;
    }
    public async Task CreateNewMatchesFromLeague()
    {
        using (var scope = _services.CreateScope())
        {
            var _context =
                scope.ServiceProvider
                    .GetRequiredService<DatabaseContext>();

            //while (_context.Leagues.Count(x => x. > 1))
            //{
            //    Console.WriteLine("**************whileloop CreateNewMatchesOfAvailableTeams");
            //    MatchController matchController = new(_context);
            //    TeamPicker teamPicker = new();
            //    await matchController.Create(teamPicker);
            //    await _context.SaveChangesAsync();
            //}
        }
    }
}
