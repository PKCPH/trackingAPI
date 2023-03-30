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
        OddsHandler oddsHandler = new OddsHandler();
        
        using (var scope = _services.CreateScope())
        {
            var _context =
                scope.ServiceProvider
                    .GetRequiredService<DatabaseContext>();

            var teams = _context.Teams.ToList();
            var TeamsWinChances = oddsHandler.WinChancesAndOdds(teams.ElementAt(0), teams.ElementAt(1), teams.ElementAt(2), teams.ElementAt(3));
            Console.WriteLine();

        }
        //MatchBackgroundTask matchBackgroundTask = new(_services);
        //matchBackgroundTask.RestartUnfinishedMatches();

        //do
        //{
        //    List<Gamematch> matches = new();
        //    Task task;
        //    using (var scope = _services.CreateScope())
        //    {
        //        var _context =
        //            scope.ServiceProvider
        //                .GetRequiredService<DatabaseContext>();

        //        matches = _context.Matches.ToList();
        //    }
        //    Console.WriteLine("TIME : " + DateTime.Now.ToString());
        //    //if any matches has not finished then play matches!
        //    //else create new matches
        //    if (matches.Where(x => x.MatchState == MatchState.NotStarted).Where(x => x.DateOfMatch < DateTime.Now).Count() > 0)
        //    {
        //        task = matchBackgroundTask.FindAndPlayMatches();
        //    }
        //    if (matches.All(x => x.MatchState == MatchState.Finished))
        //    {
        //        task = matchBackgroundTask.CreateNewMatchesOfAvailableTeams();
        //    }
        //    Console.WriteLine("ExecuteAsync loop in complete");
        //} while (await _timer.WaitForNextTickAsync(stoppingToken)
        //    && !stoppingToken.IsCancellationRequested);
    }

}