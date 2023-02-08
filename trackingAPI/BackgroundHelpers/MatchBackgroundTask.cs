using trackingAPI.Configurations;
using trackingAPI.Controllers;
using trackingAPI.Data;
using trackingAPI.Helpers;
using trackingAPI.Models;

namespace trackingAPI.BackgroundHelpers;

public class MatchBackgroundTask
{
    public async Task CreateNewMatchesFromAvailableTeams(IServiceProvider _services)
    {
        using (var scope = _services.CreateScope())
        {
            var _context =
                scope.ServiceProvider
                    .GetRequiredService<DatabaseContext>();
            while (
                _context.Teams.Count(x => (bool)x.IsAvailable) > 1)
            {
                MatchController matchController = new(_context);
                TeamPicker teamPicker = new();
                await matchController.Create(teamPicker);
                await _context.SaveChangesAsync();
                Console.WriteLine($"Number of teams that a available: {_context.Teams.Count(x => (bool)x.IsAvailable)}");
                await Task.Delay(5000);
            }
        }
    }

    //public async Task<GameMatch> ScheduleListOfMatches()
    //{
           
    //}

//public async Task ScheduledTaskOfTodaysMatches(IServiceProvider _services)
//{
//    using (var scope = _services.CreateScope())
//    {
//        var _context =
//            scope.ServiceProvider
//                .GetRequiredService<DatabaseContext>();
//        foreach ()
//        {

//        }
//    }
}
