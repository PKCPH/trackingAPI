using System.Net;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using trackingAPI.Configurations;
using trackingAPI.Data;
using trackingAPI.Helpers;
using trackingAPI.Models;

namespace trackingAPI.BackgroundHelpers;

public class ImplementBackgroundService : BackgroundService
{
    private readonly IServiceProvider _services;
    //parameter is how often the Timer ticks (set to once every second)
    private readonly PeriodicTimer _timer = new(BackgroundTaskConfiguration.TimerTickTimeSpan);

    public ImplementBackgroundService(IServiceProvider services)
    {
        _services = services;
    }

    //Task running when IHostedService starts
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using (var scope = _services.CreateScope())
        {
            var _context =
                scope.ServiceProvider
                    .GetRequiredService<DatabaseContext>();
            do
            {
                Task task;
                MatchBackgroundTask matchBackgroundTask = new(_services);
                //if any matches has not finished then play matches!
                //else create new matches

                //if (!_context.Matches.All(x => x.MatchState == MatchState.Finished))
                //{
                //    task = matchBackgroundTask.FindAndPlayMatches();
                //}
                //if (_context.Matches.All(x => x.MatchState == MatchState.Finished))
                //{
                //    task = matchBackgroundTask.CreateNewMatchesOfAvailableTeams();
                //}
                TeamPicker.SeedDistribution();
                
                await Task.Delay(1000);
                Console.WriteLine("ExecuteAsync loop in complete");
            } while (await _timer.WaitForNextTickAsync(stoppingToken)
                    && !stoppingToken.IsCancellationRequested);
        }
    }
}