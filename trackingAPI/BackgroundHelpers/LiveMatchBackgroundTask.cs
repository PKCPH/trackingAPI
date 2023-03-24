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

        while (timer.Elapsed.TotalSeconds < 5400)
        {
            TimeSpan result = TimeSpan.FromSeconds(timer.Elapsed.TotalSeconds);
            string fromTimer = result.ToString("mm':'ss");
            IsGoalScoredChance(gameMatch);
            Thread.Sleep(1000);
        }
        timer.Stop();

        var teamA = gameMatch.ParticipatingTeams.First();
        var teamB = gameMatch.ParticipatingTeams.Last();
        //if draw and draw in not allowed then play overtime
        if (teamA.TeamScore == teamB.TeamScore && !gameMatch.IsDrawAllowed) PlayOvertime(gameMatch);
        return Task.CompletedTask;
    }
    public Gamematch PlayOvertime(Gamematch gamematch)
    {
        Stopwatch timer = new Stopwatch();
        timer.Start();

        while (timer.Elapsed.TotalSeconds < 1800)
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
        var teamB = gamematch.ParticipatingTeams.Last();
        var teamBPKScore = 0;

        using (var scope = _services.CreateScope())
        {
            var _context =
                scope.ServiceProvider
                    .GetRequiredService<DatabaseContext>();

            while (teamAPKScore == teamBPKScore || rounds < 4)
            {
                int chance = rnd.Next(1, 100);
                if (chance > 45)
                {
                    teamAPKScore++;
                    teamA.TeamScore++;
                    _context.Entry(teamA).State = EntityState.Modified;
                }
                Thread.Sleep(60000);

                chance = rnd.Next(1, 100);
                if (chance > 45)
                {
                    teamBPKScore++;
                    teamB.TeamScore++;
                    _context.Entry(teamB).State = EntityState.Modified;
                }
                Thread.Sleep(60000);
                rounds++;
            }
        }
        return gamematch;
    }

    public Gamematch IsGoalScoredChance(Gamematch gameMatch)
    {
        Random rnd = new Random();
        var ballPossessionTeam = rnd.Next(10000)/100;
        bool GoalToTeamA = false;
        var chanceOfGoal = rnd.Next(1, 100);
        if (ballPossessionTeam > 100 - TeamSuperiority(gameMatch.ParticipatingTeams.First().Team,gameMatch.ParticipatingTeams.Last().Team)*100) GoalToTeamA = true;
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

    //Calculates the advantage that Team A has over Team B using a modified version of Dr. Elos chess rating equations
    public static double TeamSuperiority(Team teamA, Team teamB)
    {
        double winChance = (double)(1 / (1 + Math.Pow(10, Convert.ToDouble(teamB.Rating - teamA.Rating) / 20)));
        return winChance;
    }
}

