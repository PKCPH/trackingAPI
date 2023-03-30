using System.Threading;
using trackingAPI.Configurations;
using trackingAPI.Data;
using trackingAPI.Models;

namespace trackingAPI.BackgroundHelpers;

public class ImplementBackgroundService : IHostedService
{
    private readonly IServiceProvider _services;
    //parameter is how often the Timer ticks (set to once every second)
    private readonly PeriodicTimer _timer = new(BackgroundTaskConfiguration.TimerTickTimeSpan);

    public ImplementBackgroundService(IServiceProvider services)
    {
        _services = services;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        BackgroundTaskAsync(cancellationToken);
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        //Create timeloggers for matches here and logger for Backgroundservice (that is stopped)
        Console.WriteLine("STOPPED ASYNC IHOSTEDSERVICE");
        throw new NotImplementedException();
    }

    public async Task BackgroundTaskAsync(CancellationToken _cancellationToken)
    {
        GamematchBackgroundTask matchBackgroundTask = new(_services);
        matchBackgroundTask.RestartUnfinishedMatches();

        do
        {
            List<Gamematch> matches = new();
            Task task;
            using (var scope = _services.CreateScope())
            {
                var _context =
                    scope.ServiceProvider
                        .GetRequiredService<DatabaseContext>();

                matches = _context.Matches.ToList();
            }
            Console.WriteLine("TIME : " + DateTime.Now.ToString());
            //if any matches has not finished then play matches!
            //else create new matches
            if (matches.Where(x => x.MatchState == MatchState.NotStarted).Where(x => x.DateOfMatch < DateTime.Now).Count() > 0)
            {
                task = matchBackgroundTask.FindAndPlayMatches();
            }
            if (matches.All(x => x.MatchState == MatchState.Finished))
            {
                task = matchBackgroundTask.CreateNewMatchesOfAvailableTeams();
            }
            Console.WriteLine("ExecuteAsync loop in complete");
        } while (await _timer.WaitForNextTickAsync(_cancellationToken)
            && !_cancellationToken.IsCancellationRequested);
    }

    //Task running when IHostedService starts
    //protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    //{
    //    MatchBackgroundTask matchBackgroundTask = new(_services);
    //    matchBackgroundTask.RestartUnfinishedMatches();

    //    do
    //    {
    //        List<Gamematch> matches = new();
    //        Task task;
    //        using (var scope = _services.CreateScope())
    //        {
    //            var _context =
    //                scope.ServiceProvider
    //                    .GetRequiredService<DatabaseContext>();

    //            matches = _context.Matches.ToList();
    //        }
    //        Console.WriteLine("TIME : " + DateTime.Now.ToString());
    //        //if any matches has not finished then play matches!
    //        //else create new matches
    //        if (matches.Where(x => x.MatchState == MatchState.NotStarted).Where(x => x.DateOfMatch < DateTime.Now).Count() > 0)
    //        {
    //            task = matchBackgroundTask.FindAndPlayMatches();
    //        }
    //        if (matches.All(x => x.MatchState == MatchState.Finished))
    //        {
    //            task = matchBackgroundTask.CreateNewMatchesOfAvailableTeams();
    //        }
    //        Console.WriteLine("ExecuteAsync loop in complete");
    //    } while (await _timer.WaitForNextTickAsync(stoppingToken)
    //        && !stoppingToken.IsCancellationRequested);
    //}

}