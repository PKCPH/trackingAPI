using System.Diagnostics;
using System.Linq;
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

    public async Task FindAndPlayMatches()
    {
        DateTime now = DateTime.Now;
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
                Thread thread = new Thread(() => { PlayGameMatch(firstGameMatch); });
                thread.Start();
                Console.WriteLine($"*********THREAD #{thread.ManagedThreadId} for MATCH {firstGameMatch.Id} is started");
                Thread.Sleep(100);
            }
        }
    }

    public IOrderedEnumerable<GameMatch> GetListOfScheduledGameMatchesByDateTime()
    {
        List<GameMatch> gameMatches = new List<GameMatch>();

        using (var scope = _services.CreateScope())
        {
            var _context =
                scope.ServiceProvider
                    .GetRequiredService<DatabaseContext>();

            foreach (var match in _context.Matches.Where(x => x.MatchState == MatchState.NotStarted).ToList())
            {
                match.ParticipatingTeams = _context.MatchTeams.Where(x => x.Match.Id == match.Id).Include(x => x.Team).ToList();
                gameMatches.Add(match);
            }

        }
        var gameMatchesSortByOrder = gameMatches.OrderBy(x => x.DateOfMatch);
        return gameMatchesSortByOrder;
    }

    public async Task PlayGameMatch(GameMatch gameMatch)
    {
        Random random = new Random();
        List<MatchTeam> matchTeams = new List<MatchTeam>();
        LiveMatchBackgroundTask liveMatchBackgroundTask = new(_services);
        BetsHandler betsHandler = new(_services);
        CancellationToken stoppingToken;

        using (var scope = _services.CreateScope())
        {
            var _context =
                scope.ServiceProvider
                    .GetRequiredService<DatabaseContext>();

            await liveMatchBackgroundTask.ExecuteLiveMatch(gameMatch);
            gameMatch.MatchState = MatchState.Finished;
            await betsHandler.UpdateBalancesOnMatchFinish(gameMatch);

            //updating teams to is available
            foreach (var item in gameMatch.ParticipatingTeams)
            {
                item.Team.IsAvailable = true;
                _context.Entry(item.Team).State = EntityState.Modified;
            }
            // detach the gameMatch instance to avoid conflicts with the context
            _context.Entry(gameMatch).State = EntityState.Detached;

            // attach the updated gameMatch instance to the context and save changes
            _context.Matches.Update(gameMatch);
            await _context.SaveChangesAsync();

            _context.SaveChanges();
        }
    }
}
