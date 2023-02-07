using trackingAPI.Controllers;
using trackingAPI.Data;
using trackingAPI.Helpers;

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
            while (_context.Teams.Count(x => (bool)x.IsAvailable) > 1)
            {
                MatchController testControllerAPI = new(_context);
                TeamPicker teamPicker = new();
                await testControllerAPI.Create(teamPicker);
                await _context.SaveChangesAsync();
                Console.WriteLine($"Number of teams that a available: {_context.Teams.Count(x => (bool)x.IsAvailable)}");
                await Task.Delay(5000);
            }
        }
    }
}
