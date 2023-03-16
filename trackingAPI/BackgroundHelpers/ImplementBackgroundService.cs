using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using trackingAPI.Configurations;
using trackingAPI.Controllers;
using trackingAPI.Data;
using trackingAPI.Helpers;
using trackingAPI.Models;

namespace trackingAPI.BackgroundHelpers;

public class ImplementBackgroundService : BackgroundService
{
    private readonly IServiceProvider _services;
    //parameter is how often the Timer ticks (set to once every second)
    private readonly PeriodicTimer _timer = new(BackgroundTaskConfiguration.TimerTickTimeSpan);
    private IQueryable<Gamematch> unfinishedMatches;

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

            unfinishedMatches = _context.Matches.Where(x => x.MatchState == MatchState.Playing);


            MatchBackgroundTask matchBackgroundTask = new(_services);

            ////run through matches that wasn't finished since last time we close the program
            ////var unfinishedMatches = _context.Matches.Where(x => x.MatchState == MatchState.Playing);

            //foreach (var unfinishedMatch in this.unfinishedMatches)
            //{
            //    unfinishedMatch.ParticipatingTeams = _context.MatchTeams.Where(x => x.Match.Id == unfinishedMatch.Id)
            //        .Where(x => x.Team != null).Include(x => x.Team).ToList();
            //    await matchBackgroundTask.PlayGameMatch(unfinishedMatch);
            //    Console.WriteLine($"foreach test {unfinishedMatch.ParticipatingTeams.First().Team.Name} vs. " +
            //        $"{unfinishedMatch.ParticipatingTeams.Last().Team.Name}");
            //}

            Task task;
            do
            {
                //if any matches has not finished then play matches!
                //else create new matches
                if (!_context.Matches.All(x => x.MatchState == MatchState.Finished))
                {
                    task = matchBackgroundTask.FindAndPlayMatches();
                }
                if (_context.Matches.All(x => x.MatchState == MatchState.Finished))
                {
                    task = matchBackgroundTask.CreateNewMatchesOfAvailableTeams();
                }

                await Task.Delay(1000);
                Console.WriteLine("ExecuteAsync loop in complete");
            } while (await _timer.WaitForNextTickAsync(stoppingToken)
                && !stoppingToken.IsCancellationRequested);
        }
    }

}