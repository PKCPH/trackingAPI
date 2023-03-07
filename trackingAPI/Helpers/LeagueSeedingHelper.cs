using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using trackingAPI.Data;
using trackingAPI.Models;

namespace trackingAPI.Helpers;

public class LeagueSeedingHelper
{


    //public LeagueSeedingHelper(DatabaseContext context)
    //{
    //    this.context = context;
    //}

    //public static void DelegateSeeds(int teamNumber, League league)
    //{
    //    //delegate seeds
    //    //delegate opponents
    //    //play match
    //    //loser changes InTournament to false
    //    //delegate seeds with remaining teams where InTournament is true //maybe... cause it doesn't work branchtree wise
    //    //winner get the lowest seednumber between them and the loser
    //    //delegate new opponents
    //    //...

    //    var teams = league.Teams.Where(x => x.LeagueId == league.Id).Where(x => x.TeamSeed == 0).ToList();
    //    var count = 0;
    //    //for(int i = 0; i < teamNumber; i++)
    //    //{

    //    //}
    //    foreach (var item in teams)
    //    {
    //        item.TeamSeed = count++;
    //        Console.WriteLine($"Count is {item.TeamSeed}");
    //    }

    //}
    public League SeedDistribution(League league, DatabaseContext _context)
    {
        Random rnd = new Random();

        var eightAvailableTeams = _context.Teams.Where(t => (bool)t.IsAvailable).ToList().Take(8);

        foreach (var team in eightAvailableTeams)
        {
        

            LeagueTeam leagueTeamA = new LeagueTeam { Team = team };
            league.Teams.Add(leagueTeamA);

            //rounds.FirstOrDefault().Matches.FirstOrDefault().ParticipatingTeams.Add(matchTeamA);
            //league.Rounds.FirstOrDefault().Matches;

            //league.GameMatches.First().ParticipatingTeams.Add(matchTeamA);
            //league.GameMatches.First().ParticipatingTeams.Add(matchTeamB);

            Console.WriteLine($"LEAGUE MATCH CREATED: {leagueTeamA.Team.Name}");
        }

        return league;
    }
    //static Round[] Generate(int playersNumber)
    //{
    //    // only works for power of 2 number of players   
    //    var roundsNumber = (int)Math.Log(playersNumber, 2);
    //    var rounds = new Round[roundsNumber];
    //    for (int i = 0; i < roundsNumber; i++)
    //    {
    //        var round = new Round();
    //        var prevRound = i > 0 ? rounds[i - 1] : null;
    //        if (prevRound == null)
    //        {
    //            // if first round - result is known
    //            round.Matches = new[] {
    //                new LeagueGamematch() {
    //                    TeamASeed = 1,
    //                    TeamBSeed = 2
    //                }
    //            };
    //        }
    //        else
    //        {
    //            round.Matches = new LeagueGamematch[prevRound.Matches.Length * 2];
    //            // find median. For round 2 there are 4 players and median is 2.5 (between 2 and 3)
    //            // for round 3 there are 8 players and median is 4.5 (between 4 and 5)
    //            var median = (round.Matches.Length * 2 + 1) / 2f;
    //            var next = 0;
    //            foreach (var match in prevRound.Matches)
    //            {
    //                // you can play here by switching PlayerA and PlayerB or reordering stuff
    //                round.Matches[next] = new LeagueGamematch()
    //                {
    //                    TeamASeed = match.TeamASeed,
    //                    TeamBSeed = (int)(median + Math.Abs(match.TeamASeed - median)),
    //                    //Rounds = roundsNumber
    //                };
    //                next++;
    //                round.Matches[next] = new LeagueGamematch()
    //                {
    //                    TeamASeed = match.TeamBSeed,
    //                    TeamBSeed = (int)(median + Math.Abs(match.TeamBSeed - median))
    //                };
    //                next++;
    //            }
    //        }
    //        rounds[i] = round;
    //    }
    //    return rounds.Reverse().ToArray();
    //}

    ////generating bracketrounds for single elimination tournaments
    //public static ICollection<Round> Generate(int teamNumber)
    //{
    //    // only works for power of 2 number of players   
    //    var roundsNumber = (int)Math.Log(teamNumber, 2);
    //    var rounds = new Round[roundsNumber];
    //    for (int i = 0; i < roundsNumber; i++)
    //    {
    //        var round = new Round();
    //        //if prevRounds = is more than 0, give i-1 else null  
    //        var prevRound = i > 0 ? rounds[i - 1] : null;
    //        if (prevRound == null)
    //        {
    //            // if first round - result is known
    //            round.Matches = new[] {
    //                new GameMatch() {
    //                    TeamASeed = 1,
    //                    TeamBSeed = 2
    //                }
    //            };
    //        }
    //        else
    //        {
    //            round.Matches = new GameMatch[prevRound.Matches.Count * 2];

    //            // find median. For round 2 there are 4 players and median is 2.5 (between 2 and 3)
    //            // for round 3 there are 8 players and median is 4.5 (between 4 and 5)
    //            var median = (round.Matches.Count * 2 + 1) / 2f;
    //            var next = 0;
    //            foreach (var match in prevRound.Matches)
    //            {
    //                // you can play here by switching PlayerA and PlayerB or reordering stuff
    //                round.Matches.ToArray()[next] = new GameMatch()
    //                {

    //                    //_context.Entry(match.TeamASeed),
    //                    TeamASeed = match.TeamASeed,
    //                    TeamBSeed = (int)(median + Math.Abs(match.TeamASeed - median))
    //                };
    //                next++;
    //                round.Matches.ToArray()[next] = new GameMatch()
    //                {
    //                    TeamASeed = match.TeamBSeed,
    //                    TeamBSeed = (int)(median + Math.Abs(match.TeamBSeed - median))
    //                };
    //                next++;
    //            }
    //            //_context.Entry(match.TeamASeed).State = EntityState.Modified;
    //            //_context.Entry(round.Matches[next].TeamASeed).State = EntityState.Modified;
    //            //_context.SaveChanges();
    //        }
    //        rounds[i] = round;
    //    }
    //    return (ICollection<Round>)rounds.Reverse();
    //}


    //var rounds = Generate(8);
    //foreach (var item in rounds[0].Matches)
    //{
    //    for (int i = 0; i <= 8; i++)
    //    {
    //        var team1 = teams[i];
    //        i++;
    //        var team2 = teams[i];
    //        MatchTeam matchTeamA = new MatchTeam { Team = team1, TeamId = team1.Id };
    //        MatchTeam matchTeamB = new MatchTeam { Team = team2, TeamId = team2.Id };

    //        item.ParticipatingTeams.Add(matchTeamA);
    //        item.ParticipatingTeams.Add(matchTeamB);
    //    }
    //}


    //foreach (var round in rounds)
    //{
    //    foreach (var match in round.Matches)
    //    {
    //        Console.WriteLine("{0} vs {1}", match.TeamASeed, match.TeamBSeed);
    //        using (var scope = _service.CreateScope())
    //        {
    //            var _context =
    //                scope.ServiceProvider
    //                    .GetRequiredService<DatabaseContext>();

    //            _context.LeagueTeams.Where(x => x.TeamId == match.ParticipatingTeams.First().TeamId);

    //        }
    //    }
    //    Console.WriteLine();
    //}

}
