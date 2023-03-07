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
    //public async Task CreateNewLeagueOfAvailableTeams()
    //{
    //    using (var scope = _services.CreateScope())
    //    {
    //        var _context =
    //            scope.ServiceProvider
    //                .GetRequiredService<DatabaseContext>();

    //        while (_context.Teams.Count(x => (bool)x.IsAvailable) > 1)
    //        {
    //            //LeagueController leagueController = new(_context);
    //            LeagueSeedingHelper leagueSeedingHelper = new();
    //            var newLeague = leagueSeedingHelper.SeedDistribution(_context);
    //            _context.Leagues.Add(newLeague);
    //            //await leagueController.Create(newLeague);
    //            await _context.SaveChangesAsync();
    //        }
    //    }
    //}

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

    public IOrderedEnumerable<Gamematch> GetListOfScheduledGameMatchesByDateTime()
    {
        List<Gamematch> gamematches = new List<Gamematch>();

        using (var scope = _services.CreateScope())
        {
            var _context =
                scope.ServiceProvider
                    .GetRequiredService<DatabaseContext>();
            foreach (var match in _context.Matches.Where(x => x.MatchState == MatchState.NotStarted))
            {
                var match2 = AddTeamsToParticipatingTeams(match, match.Id);
                gamematches.Add(match2);
            }
        }
        var gamematchesSortByOrder = gamematches.OrderBy(x => x.DateOfMatch);

        return gamematchesSortByOrder;
    }

    public Gamematch AddTeamsToParticipatingTeams(Gamematch gameMatch, Guid matchId)
    {
        using (var scope = _services.CreateScope())
        {
            var _context =
                scope.ServiceProvider
                    .GetRequiredService<DatabaseContext>();
            var matches = _context.MatchTeams.Where(x => x.Match.Id == matchId).ToList();
            gameMatch.ParticipatingTeams.Add(matches.First());
            gameMatch.ParticipatingTeams.Add(matches.Last());
        }
        return gameMatch;

    }

    public Task PlayGameMatch(Gamematch gameMatch)
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

            liveMatchBackgroundTask.ExecuteLiveMatch(gameMatch);
            gameMatch.MatchState = MatchState.Finished;
            _context.Entry(gameMatch).State = EntityState.Modified;
            
            foreach (var item in _context.MatchTeams)
            {
                matchTeams.Add(item);
            }
            //foreach matchTeams where MatchId is matching the selected gameMatch.Id
            foreach (var item2 in matchTeams.Where(x => x.Match.Id == gameMatch.Id))
            {
                //foreach team.id that is matching with matchTeams.teamId
                foreach (var item3 in _context.Teams.Where(x => x.Id == item2.Team.Id)) item3.IsAvailable = true;
            }
            _context.SaveChanges();
        }
        return Task.CompletedTask;
    }
}
