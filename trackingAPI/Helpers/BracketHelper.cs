using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using trackingAPI.Models;

namespace trackingAPI.Helpers;

public static class BracketHelper
{
    //public static void Play(ICollection<Team> teams)
    //{
    //    // Create the tournament tree
    //    List<List<Gamematch>> rounds = new List<List<Gamematch>>();

    //    // Create the first round
    //    List<Gamematch> firstRound = new List<Gamematch>();
    //    int teamCount = teams.Count;
    //    Team[] teamArray = new Team[teamCount];
    //    teams.CopyTo(teamArray, 0);
    //    for (int i = 0; i < teamCount; i += 2)
    //    {
    //        var match = new Gamematch
    //        {
    //            ParticipatingTeams = new List<MatchTeam>
    //        {
    //            new MatchTeam {Team = teamArray[i], Seed = i + 1},
    //            new MatchTeam {Team = teamArray[i + 1], Seed = i + 2}
    //        }
    //        };
    //        firstRound.Add(match);
    //    }
    //    rounds.Add(firstRound);

    //    // Play each round until we have a winner
    //    while (rounds[rounds.Count - 1].Count > 1)
    //    {
    //        // Create the next round
    //        List<Gamematch> nextRound = new List<Gamematch>();
    //        foreach (var match in rounds[rounds.Count - 1])
    //        {
    //            // If the match hasn't been played yet, play it and set the winner
    //            if (match.MatchState == MatchState.NotStarted)
    //            {
    //                match.MatchState = MatchState.Playing;
    //                match.ParticipatingTeams = match.ParticipatingTeams
    //                    .Select(mt => new MatchTeam { Team = mt.Team, Seed = mt.Seed })
    //                    .ToList();
    //                match.ParticipatingTeams.ElementAt(0).TeamScore = RandomNumber(0, 10);
    //                match.ParticipatingTeams.ElementAt(1).TeamScore = RandomNumber(0, 10);
    //                match.Winner = match.ParticipatingTeams.ElementAt(0).TeamScore > match.ParticipatingTeams.ElementAt(1).TeamScore
    //                    ? match.ParticipatingTeams.ElementAt(0).Team
    //                    : match.ParticipatingTeams.ElementAt(1).Team;
    //                match.MatchState = MatchState.Finished;
    //            }

    //            // Add the winner of the match to the next round
    //            nextRound.Add(new Gamematch { ParticipatingTeams = new List<MatchTeam> { new MatchTeam { Team = match.Winner, Seed = match.WinnerSeed } } });
    //        }
    //        rounds.Add(nextRound);
    //    }

    //    // We have a winner!
    //    Console.WriteLine($"Winner: {rounds[rounds.Count - 1][0].Winner.Name}");
    //}

    private static int RandomNumber(int min, int max)
    {
        Random random = new Random();
        return random.Next(min, max + 1);
    }
}
