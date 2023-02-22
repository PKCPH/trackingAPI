using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using trackingAPI.Controllers;
using trackingAPI.Data;
using trackingAPI.Helpers;
using trackingAPI.Models;

namespace trackingAPI.BackgroundHelpers;

public class MatchBackgroundTask
{
    private readonly IServiceProvider _services;
    public MatchBackgroundTask(IServiceProvider services)
    {
        _services = services;
    }
    public async Task CreateNewMatchesOfAvailableTeams()
    {
        using (var scope = _services.CreateScope())
        {
            var _context =
                scope.ServiceProvider
                    .GetRequiredService<DatabaseContext>();

            while (_context.Teams.Count(x => (bool)x.IsAvailable) > 1)
            {
                MatchController matchController = new(_context);
                TeamPicker teamPicker = new();
                await matchController.Create(teamPicker);
                await _context.SaveChangesAsync();
            }
        }
    }

    public Task FindAndPlayMatches()
    {
        DateTime now = DateTime.Now;
        //var matches = GetListOfScheduledGameMatchesByDateTime();
        //while any matches has passed the datetime.now
        while (GetListOfScheduledGameMatchesByDateTime().Any(x => x.DateOfMatch < now))
        {
            var firstGameMatch = GetListOfScheduledGameMatchesByDateTime().OrderBy(x => x.DateOfMatch).First();
            // while now has past the schedule time of the match  
            if (now > firstGameMatch.DateOfMatch)
            {
                using (var scope = _services.CreateScope())
                {
                    var _context =
                        scope.ServiceProvider
                            .GetRequiredService<DatabaseContext>();
                    _context.Entry(firstGameMatch).State = EntityState.Modified;
                    _context.SaveChanges();
                }
                //new thread is created and started per livematch
                Thread thread = new Thread(() => { PlayGameMatch(firstGameMatch); });
                thread.Start();
                Console.WriteLine($"*********THREAD #{thread.ManagedThreadId} for MATCH {firstGameMatch.Id} is started");
                Thread.Sleep(100);
            }
        }
        return Task.CompletedTask;
    }

    public IOrderedEnumerable<GameMatch> GetListOfScheduledGameMatchesByDateTime()
    {
        List<GameMatch> gameMatches = new List<GameMatch>();

        using (var scope = _services.CreateScope())
        {
            var _context =
                scope.ServiceProvider
                    .GetRequiredService<DatabaseContext>();
            foreach (var item in _context.Matches.Where(x => x.MatchState == MatchState.NotStarted))
            {
                gameMatches.Add(item);
            }
        }
        var gameMatchesSortByOrder = gameMatches.OrderBy(x => x.DateOfMatch);

        return gameMatchesSortByOrder;
    }

    public Task PlayGameMatch(GameMatch gameMatch)
    {
        Random random = new Random();
        List<MatchTeam> matchTeams = new List<MatchTeam>();
        LiveMatchBackgroundTask liveMatchBackgroundTask = new(_services);
        CancellationToken stoppingToken;

        using (var scope = _services.CreateScope())
        {
            var _context =
                scope.ServiceProvider
                    .GetRequiredService<DatabaseContext>();

            //make cautios of a game that been paused of postponed and will resume another time
            foreach (var item in _context.Matches.Where(x => x.Id == gameMatch.Id))
            {
                liveMatchBackgroundTask.ExecuteLiveMatch(item);
                item.MatchState = MatchState.Finished;
                _context.Entry(item).State = EntityState.Modified;
            }
            foreach (var item in _context.MatchTeams)
            {
                matchTeams.Add(item);
            }
            //foreach matchTeams where MatchId is matching the selected gameMatch.Id
            foreach (var item2 in matchTeams.Where(x => x.MatchId == gameMatch.Id))
            {
                //foreach team.id that is matching with matchTeams.teamId
                foreach (var item3 in _context.Teams.Where(x => x.Id == item2.TeamId)) item3.IsAvailable = true;
            }
            _context.SaveChanges();
        }
        return Task.CompletedTask;
    }
}
