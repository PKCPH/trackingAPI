using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using trackingAPI.Configurations;
using trackingAPI.Data;
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

    //Task running when IHostedSErvice starts
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
                //if loop to check if all matches has been played before creating new matches,
                //so teams wont be matched with the same opponent
                if (_context.Matches.Any(x => x.MatchState == MatchState.NotStarted))
                {
                    task = matchBackgroundTask.FindAndPlayMatches();
                }
                else
                {
                    task = matchBackgroundTask.CreateNewMatchesOfAvailableTeams();
                }

                await Task.WhenAny(task);

                Console.WriteLine("end");
            } while (await _timer.WaitForNextTickAsync(stoppingToken)
                    && !stoppingToken.IsCancellationRequested);
        }
    }
}