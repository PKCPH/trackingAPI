using System.Diagnostics;
using System.Timers;
using trackingAPI.Configurations;
using trackingAPI.Data;
using trackingAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace trackingAPI.BackgroundHelpers;


public class LiveMatchBackgroundTask
{
    private readonly IServiceProvider _services;
    public LiveMatchBackgroundTask(IServiceProvider services)
    {
        _services = services;
    }

    public Task ExecuteLiveMatch(GameMatch gameMatch)
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

        while (timer.Elapsed.TotalSeconds < 60)
        {
            TimeSpan result = TimeSpan.FromSeconds(timer.Elapsed.TotalSeconds);
            string fromTimer = result.ToString("mm':'ss");

            var game = IsGoalScoredChance(gameMatch);
            Console.WriteLine($"Match: {game.Id} Time: {fromTimer}");

            Console.WriteLine();

            Thread.Sleep(1000);
        }
        timer.Stop();

        //overtime
        //if(
        //    gameMatch.ParticipatingTeams.First().TeamScore == gameMatch.ParticipatingTeams.Last().TeamScore 
        //    && !gameMatch.IsDrawAllowed)
        //{
        //    timer.Start();

        //    while (timer.Elapsed.TotalSeconds < 10)
        //    {
        //        TimeSpan result = TimeSpan.FromSeconds(timer.Elapsed.TotalSeconds);
        //        string fromTimer = result.ToString("mm':'ss");

        //        IsGoalScoredChance(gameMatch);
        //        Console.WriteLine($"Match: {gameMatch.Id} Time: {fromTimer}");

        //        Console.WriteLine();

        //        Thread.Sleep(1000);
        //    }
        //    timer.Stop();
        //}
        using (var scope = _services.CreateScope())
        {
            var _context =
                scope.ServiceProvider
                    .GetRequiredService<DatabaseContext>();
            if (gameMatch.ParticipatingTeams.First().TeamScore > gameMatch.ParticipatingTeams.Last().TeamScore)
            {
                gameMatch.ParticipatingTeams.First().Result = Result.Winner;
                gameMatch.ParticipatingTeams.Last().Result = Result.Loser;
            }
            else if (gameMatch.ParticipatingTeams.First().TeamScore < gameMatch.ParticipatingTeams.Last().TeamScore)
            {
                gameMatch.ParticipatingTeams.First().Result = Result.Loser;
                gameMatch.ParticipatingTeams.Last().Result = Result.Winner;
            }
            else if (gameMatch.ParticipatingTeams.First().TeamScore == gameMatch.ParticipatingTeams.Last().TeamScore)
            {
                gameMatch.ParticipatingTeams.First().Result = Result.Draw;
                gameMatch.ParticipatingTeams.Last().Result = Result.Draw;
            }
            _context.Entry(gameMatch).State = EntityState.Modified;
            _context.SaveChanges();
        }
        return Task.CompletedTask;
    }

    public GameMatch IsGoalScoredChance(GameMatch gameMatch)
    {
        Random rnd = new Random();
        var ballPossessionTeam = rnd.Next(100);
        bool GoalToTeamA = false;
        var chanceOfGoal = rnd.Next(1, 100);
        if (ballPossessionTeam < 50) GoalToTeamA = true;
        if (chanceOfGoal > 99) return gameMatch;

        Console.WriteLine($"GOAL IS SCORED");
        using (var scope = _services.CreateScope())
        {
            var _context =
                scope.ServiceProvider
                    .GetRequiredService<DatabaseContext>();
            if (GoalToTeamA)
            {
                gameMatch.ParticipatingTeams.First().TeamScore++;
                _context.Entry(gameMatch.ParticipatingTeams.First()).State = EntityState.Modified;
                _context.SaveChanges();
            }
            else
            {
                gameMatch.ParticipatingTeams.Last().TeamScore++;
                //_context.Entry(gameMatch.ParticipatingTeams.Last()).State = EntityState.Modified;
                _context.SaveChanges();
            }
        }
        return gameMatch;
    }
    //sup!
}

