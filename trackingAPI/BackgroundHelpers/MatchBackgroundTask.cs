using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
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

            var n = 4;

            for (int i = 0; i <= n; i++)
            {
                if (_context.Teams.Count(x => (bool)x.IsAvailable) > 1)
                {
                    TeamPicker teamPicker = new();
                    await _context.Matches.AddAsync(teamPicker.CreateMatch(_context));
                    await _context.SaveChangesAsync();
                }
            }
        }
    }
    //finding matches to play and executing the PlayGameMatch() in an seperate thread
    public Task FindAndPlayMatches()
    {
        DateTime now = DateTime.Now;
        foreach (var item in GetListOfScheduledGameMatchesByDateTime().Where(x => x.DateOfMatch < now))
        {
            if (item.ParticipatingTeams.Count != 2) continue;
            Thread thread = new Thread(() => { PlayGameMatch(item); });
            thread.Start();
            Console.WriteLine($"*********THREAD #{thread.ManagedThreadId} for MATCH {item.Id} is started");
            Thread.Sleep(100);
        }
        return Task.CompletedTask;
    }

    public IOrderedEnumerable<Gamematch> GetListOfScheduledGameMatchesByDateTime()
    {
        List<Gamematch> gameMatches = new List<Gamematch>();

        using (var scope = _services.CreateScope())
        {
            var _context =
                scope.ServiceProvider
                    .GetRequiredService<DatabaseContext>();

            foreach (var match in _context.Matches.Where(x => x.MatchState == MatchState.NotStarted).ToList())
            {
                match.ParticipatingTeams = _context.MatchTeams.Where(x => x.Match.Id == match.Id)
                    .Where(x => x.Team != null).Include(x => x.Team).ToList();
                gameMatches.Add(match);
            }
        }
        var gameMatchesSortByOrder = gameMatches.OrderBy(x => x.DateOfMatch);
        return gameMatchesSortByOrder;
    }

    //if program is restarted
    public async Task RestartUnfinishedMatches()
    {
        using (var scope = _services.CreateScope())
        {
            var _context =
                scope.ServiceProvider
                    .GetRequiredService<DatabaseContext>();

            foreach (var match in _context.Matches.Where(x => x.MatchState != MatchState.Finished && x.MatchState != MatchState.NotStarted).ToList())
            {
                match.ParticipatingTeams = _context.MatchTeams.Where(x => x.Match.Id == match.Id)
                    .Where(x => x.Team != null).Include(x => x.Team).ToList();
                match.ParticipatingTeams.First().TeamScore = 0;
                match.ParticipatingTeams.Last().TeamScore = 0;
                match.MatchState = MatchState.NotStarted;
                _context.Entry(match).State = EntityState.Modified;
                _context.SaveChanges();
            }
        }
    }

    //called in FindAndPlayMatches()
    public async Task PlayGameMatch(Gamematch gamematch)
    {
        Random random = new Random();
        List<MatchTeam> matchTeams = new List<MatchTeam>();
        LiveMatchBackgroundTask liveMatchBackgroundTask = new(_services);
        BetsHandler betsHandler = new(_services);
        CancellationToken stoppingToken;
        //If match has a BYE team
        if (gamematch.ParticipatingTeams.Any(x => x.Team.Name == "BYE")) { ExecuteByeMatch(gamematch); }
        else { await liveMatchBackgroundTask.ExecuteLiveMatch(ref gamematch); }

        gamematch.MatchState = MatchState.Finished;
        UpdateFinishedMatchInDatabase(gamematch);
        await betsHandler.UpdateBalancesOnMatchFinish(gamematch);

        using (var scope = _services.CreateScope())
        {
            var _context =
                scope.ServiceProvider
                    .GetRequiredService<DatabaseContext>();
            if (gamematch.LeagueId == null)
            {
                //updating teams to is available
                foreach (var item in gamematch.ParticipatingTeams)
                {
                    item.Team.IsAvailable = true;
                    _context.Entry(item.Team).State = EntityState.Modified;
                }
                // detach the gameMatch instance to avoid conflicts with the context
                _context.Entry(gamematch).State = EntityState.Detached;
            }
            else if (gamematch.ParticipatingTeams.Where(x => x.Result == Result.Loser).First().Team != null)
            {
                var team = gamematch.ParticipatingTeams.Where(x => x.Result == Result.Loser).First().Team;
                team.IsAvailable = true;
                _context.Entry(team).State = EntityState.Modified;
            }

            // attach the updated gameMatch instance to the context and save changes
            _context.Matches.Update(gamematch);
            await _context.SaveChangesAsync();
            _context.SaveChanges();
        }
    }

    //Deletes BYE team and set opponent to winner, match will not be simulated
    private Task ExecuteByeMatch(Gamematch gamematch)
    {
        using (var scope = _services.CreateScope())
        {
            var _context =
                scope.ServiceProvider
                    .GetRequiredService<DatabaseContext>();
            var team = gamematch.ParticipatingTeams.Where(x => x.Team.Name != "BYE").First();
            var teamBye = gamematch.ParticipatingTeams.Where(x => x.Team.Name == "BYE").First();
            team.Result = Result.Winner;
            teamBye.Result = Result.Loser;
            _context.Entry(team).State = EntityState.Modified;
            _context.Entry(teamBye).State = EntityState.Modified;
            _context.Entry(teamBye.Team).State = EntityState.Deleted;
            _context.Matches.Update(gamematch);
            _context.SaveChanges();
        }
        return Task.CompletedTask;
    }

    //replace instead of in match and bye team matches post finished
    private Task UpdateFinishedMatchInDatabase(Gamematch gamematch)
    {
        var teamA = gamematch.ParticipatingTeams.First();
        var teamB = gamematch.ParticipatingTeams.Last();
        using (var scope = _services.CreateScope())
        {
            var _context =
                scope.ServiceProvider
                    .GetRequiredService<DatabaseContext>();

            if (teamA.TeamScore > teamB.TeamScore) { teamA.Result = Result.Winner; teamB.Result = Result.Loser; }
            if (teamA.TeamScore < teamB.TeamScore) { teamA.Result = Result.Loser; teamB.Result = Result.Winner; }
            if (teamA.TeamScore == teamB.TeamScore && gamematch.IsDrawAllowed) { teamA.Result = Result.Draw; teamB.Result = Result.Draw; }

            _context.Entry(teamA).State = EntityState.Modified;
            _context.Entry(teamB).State = EntityState.Modified;
            _context.SaveChanges();

            if (gamematch.LeagueId == null) return Task.CompletedTask;

            var nextRound = gamematch.Round;
            nextRound--;
            var league = _context.Leagues.Where(x => x.Id == gamematch.LeagueId).First();
            if (nextRound == 0)
            {
                var winner = gamematch.ParticipatingTeams.Where(x => x.Result == Result.Winner).First().Team;
                league.LeagueState = LeagueState.Finished;
                winner.IsAvailable = true;
                _context.Entry(league).State = EntityState.Modified;
                _context.Entry(winner).State = EntityState.Modified;
                _context.SaveChanges();
                return Task.CompletedTask;
            }

            //takes lowest int of Seeds
            var winnerSeed = Math.Min(Convert.ToByte(teamA.Seed), Convert.ToByte(teamB.Seed));

            var nextMatchTeam = _context.MatchTeams.Where(x => x.Match.Round == nextRound)
                .Where(x => x.Seed == winnerSeed).Where(x => x.Match.LeagueId == league.Id).First();
            
            //adding winning team
            nextMatchTeam.Team = gamematch.ParticipatingTeams.Where(x => x.Result == Result.Winner).First().Team;
            //updating to sql
            _context.Entry(nextMatchTeam.Team).State = EntityState.Modified;
            _context.SaveChanges();

            return Task.CompletedTask;
        }
    }
}
