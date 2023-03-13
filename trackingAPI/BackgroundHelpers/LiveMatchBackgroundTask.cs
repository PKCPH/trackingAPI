using System.Diagnostics;
using System.Timers;
using trackingAPI.Configurations;
using trackingAPI.Data;
using trackingAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.ComponentModel.DataAnnotations;

namespace trackingAPI.BackgroundHelpers;


public class LiveMatchBackgroundTask
{
    private readonly IServiceProvider _services;
    public LiveMatchBackgroundTask(IServiceProvider services)
    {
        _services = services;
    }

    public Task ExecuteLiveMatch(Gamematch gameMatch)
    {
        using (var scope = _services.CreateScope())
        {
            var _context =
                scope.ServiceProvider
                    .GetRequiredService<DatabaseContext>();
            gameMatch.MatchState = MatchState.Playing;

            _context.Entry(gameMatch).State = EntityState.Modified;
            _context.SaveChanges();
        }

        Stopwatch timer = new Stopwatch();
        timer.Start();

        while (timer.Elapsed.TotalSeconds < 20)
        {
            TimeSpan result = TimeSpan.FromSeconds(timer.Elapsed.TotalSeconds);
            string fromTimer = result.ToString("mm':'ss");

            IsGoalScoredChance(gameMatch);
            Console.WriteLine($"Match: {gameMatch.Id} Time: {fromTimer}");

            Console.WriteLine();

            Thread.Sleep(1000);
        }
        timer.Stop();

        var teamA = gameMatch.ParticipatingTeams.First();
        var teamB = gameMatch.ParticipatingTeams.Last();
        //if draw and draw in not allowed then play overtime
        if (teamA.TeamScore == teamB.TeamScore && !gameMatch.IsDrawAllowed) PlayOvertime(gameMatch);

        using (var scope = _services.CreateScope())
        {
            var _context =
                scope.ServiceProvider
                    .GetRequiredService<DatabaseContext>();

            if (teamA.TeamScore > teamB.TeamScore) { teamA.Result = Result.Winner; teamB.Result = Result.Loser; }
            if (teamA.TeamScore < teamB.TeamScore) { teamA.Result = Result.Loser; teamB.Result = Result.Winner; }
            if (teamA.TeamScore == teamB.TeamScore && gameMatch.IsDrawAllowed) { teamA.Result = Result.Draw; teamB.Result = Result.Draw; }

            _context.Entry(teamA).State = EntityState.Modified;
            _context.Entry(teamB).State = EntityState.Modified;
            _context.SaveChanges();

            if (gameMatch.LeagueId == null) return Task.CompletedTask;

            var nextRound = gameMatch.ParticipatingTeams.Where(x => x.Id == teamA.Id).First().Round;
            nextRound--;
            if (nextRound == 0)
            {

                var league = _context.Leagues.Where(x => x.Id == gameMatch.LeagueId).First();
                league.LeagueState = LeagueState.Finished;
                _context.Entry(league).State = EntityState.Modified;
                _context.SaveChanges();

                var winner = gameMatch.ParticipatingTeams.Where(x => x.Result == Result.Winner).First().Team;
                winner.IsAvailable = true;
                _context.Entry(winner).State = EntityState.Modified;
                _context.SaveChanges();
                return Task.CompletedTask;
            }


            //takes lowest int of Seeds
            var winnerSeed = Math.Min(Convert.ToByte(teamA.Seed), Convert.ToByte(teamB.Seed));
            var nextMatchTeam = _context.MatchTeams.Where(x => x.Round == nextRound).Where(x => x.Seed == winnerSeed).First();
            //adding winning team
            nextMatchTeam.Team = gameMatch.ParticipatingTeams.Where(x => x.Result == Result.Winner).First().Team;
            //updating to sql
            _context.Entry(nextMatchTeam.Team).State = EntityState.Modified;
            _context.SaveChanges();
        }
        return Task.CompletedTask;
    }

    public Gamematch PlayOvertime(Gamematch gamematch)
    {
        Stopwatch timer = new Stopwatch();
        timer.Start();

        while (timer.Elapsed.TotalSeconds < 5)
        {
            TimeSpan result = TimeSpan.FromSeconds(timer.Elapsed.TotalSeconds);
            string fromTimer = result.ToString("mm':'ss");

            IsGoalScoredChance(gamematch);
            Console.WriteLine($"Match overtime: {gamematch.Id} Time: {fromTimer}");

            Console.WriteLine();

            Thread.Sleep(1000);
        }
        timer.Stop();

        if (gamematch.ParticipatingTeams.First().TeamScore != gamematch.ParticipatingTeams.Last().TeamScore) return gamematch;

        Console.WriteLine($"Match Penalty shootout: {gamematch.Id}");
        PlayPenaltyShootout(gamematch);


        return gamematch;
    }

    public Gamematch PlayPenaltyShootout(Gamematch gamematch)
    {
        Stopwatch timer = new Stopwatch();
        Random rnd = new Random();
        var rounds = 0;

        var teamA = gamematch.ParticipatingTeams.First();
        var teamAPKScore = 0;
        var teamAChance = 0;

        var teamB = gamematch.ParticipatingTeams.Last();
        var teamBChance = 0;
        var teamBPKScore = 0;

        using (var scope = _services.CreateScope())
        {
            var _context =
                scope.ServiceProvider
                    .GetRequiredService<DatabaseContext>();

            while (teamAPKScore == teamBPKScore || rounds < 4)
            {
                teamAChance = rnd.Next(1, 100);
                if (teamAChance > 45)
                {
                    teamAPKScore++;
                    teamA.TeamScore++;
                    _context.Entry(teamA).State = EntityState.Modified;
                    Console.WriteLine($"{teamA.Team.Name} SCORED! PK SCORE: {teamA.Team.Name} - {teamAPKScore} VS {teamB.Team.Name} - {teamBPKScore}");
                }
                Thread.Sleep(1000);

                teamBChance = rnd.Next(1, 100);
                if (teamBChance > 45)
                {
                    teamBPKScore++;
                    teamB.TeamScore++;
                    _context.Entry(teamB).State = EntityState.Modified;
                    Console.WriteLine($"{teamB.Team.Name} SCORED! PK SCORE: {teamA.Team.Name} - {teamAPKScore} VS {teamB.Team.Name} - {teamBPKScore}");
                }
                Thread.Sleep(1000);
                rounds++;
            }
        }
        return gamematch;
    }

    public Gamematch IsGoalScoredChance(Gamematch gameMatch)
    {
        Random rnd = new Random();
        var ballPossessionTeam = rnd.Next(100);
        bool GoalToTeamA = false;
        var chanceOfGoal = rnd.Next(1, 100);
        if (ballPossessionTeam < 50) GoalToTeamA = true;
        if (chanceOfGoal > 1) return gameMatch;

        Console.WriteLine($"GOAL IS SCORED");
        using (var scope = _services.CreateScope())
        {
            var _context =
                scope.ServiceProvider
                    .GetRequiredService<DatabaseContext>();
            //if teamA is scoring then Teamscore++ other the same for teamB
            if (GoalToTeamA)
            {
                gameMatch.ParticipatingTeams.First().TeamScore++;
                _context.Entry(gameMatch.ParticipatingTeams.First()).State = EntityState.Modified;
            }
            else
            {
                gameMatch.ParticipatingTeams.Last().TeamScore++;
                _context.Entry(gameMatch.ParticipatingTeams.Last()).State = EntityState.Modified;
            }
            _context.SaveChanges();
        }
        return gameMatch;
    }
}

