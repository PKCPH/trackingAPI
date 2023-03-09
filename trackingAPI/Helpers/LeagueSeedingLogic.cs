using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using trackingAPI.Data;
using trackingAPI.Models;

namespace trackingAPI.Helpers;

public class LeagueSeedingLogic
{
    //Order list of teams randomly
    //check if list is big enough, if not add byes - 2*2*2*2 - 2^4
    //create first round of matchups
    //create the rest of the rounds
    public League CreateRounds(League league, DatabaseContext _context)
    {
        
        LeagueSeedingHelper leagueSeedingHelper = new LeagueSeedingHelper();
        //TeamController teamController = new(_context);
        //var teams = teamController.Get().Result;
        var leagueTeams = leagueSeedingHelper.GetListOfEightTeams(league, _context);

        league.Teams = leagueTeams.Teams.ToList();
        league.StartDate = DateTime.Now;
        league.LeagueState = LeagueState.NotStarted;
        league.Name = "testleague123";

        LeagueSeedingLogic leagueSeedingLogic = new LeagueSeedingLogic();

        //leagueSeedingLogic.CreateRounds(ref league);

        //List<Gamematch> gamematches1 = new List<Gamematch>();
        ////league.Gamematches= gamematches1;
        //MatchTeam matchTeam = new();
        var randomizedTeams = RandomizeTeamOrder(league.Teams.ToList());

        int rounds = FindNumberOfRounds(randomizedTeams.Count);
        //int byes = NumberOfByes(rounds, randomizedTeams.Count);

        //var matches = GenerateTournamentMatches(,league);
        Console.WriteLine();
        //BracketHelper.Play(randomizedTeams);

        var teams = randomizedTeams.Select(x => x.Team).Where(x => x.IsAvailable == true).ToList();

        //int[] array = new int[] { };

        var roundsNumber = 0;
        var gamematches = CreateFirstRound(ref teams, out roundsNumber);

        //adding the first round to league
        //league.Rounds.Add(CreateFirstRound(byes, randomizedTeams));
        Console.WriteLine();
        CreateOtherRounds(ref gamematches, teams, roundsNumber);

        league.Gamematches= gamematches;

        return league;
        //foreach (var item in gamematches)
        //{
        //    league.Gamematches.Add(item);
        //}
    }
    private void CreateOtherRounds(ref List<Gamematch> gamematches, List<Team> teams, int roundsNumber)
    {
        Random rnd = new Random();
        var teamsCount = teams.Count/2;
        //var qualTeams = teamsCount / 2;
        while (roundsNumber > 0)
        {
            var maxTeamCount = teamsCount;
            teamsCount /= 2;
            //var teamLoopCount = teams.Count/2;
            for (int i = 1; i <= teamsCount; i++)
            {
                Gamematch gamematch = new()
                {
                    ParticipatingTeams = new List<MatchTeam>(),
                    Round = roundsNumber
                };

                MatchTeam matchTeamA = new MatchTeam { Team = null, Seed = i };
                MatchTeam matchTeamB = new MatchTeam { Team = null, Seed = maxTeamCount };
                maxTeamCount--;
                gamematch.ParticipatingTeams.Add(matchTeamA);
                gamematch.ParticipatingTeams.Add(matchTeamB);
                gamematch.DateOfMatch = DateTimePicker.CreateRandomMatchTime();

                gamematches.Add(gamematch);

                if (roundsNumber == 1) i = (int)teamsCount;
            }
            roundsNumber--;
        }
    }
    private List<Gamematch> CreateFirstRound(ref List<Team> teams, out int roundsNumber)
    {
        roundsNumber = (int)Math.Log(teams.Count, 2);
        //var rounds = new Array[roundsNumber];
        List<Gamematch> gamematches = new List<Gamematch>();
        Random rnd = new();
        var maxTeamCount = teams.Count;
        var teamLoopCount = teams.Count/2;
        //change int here to half
        for (int i = 1; i <= teamLoopCount; i++)
        {
            Gamematch gamematch = new()
            {
                ParticipatingTeams = new List<MatchTeam>(),
                Round = roundsNumber
            };

            var TwoRandomAvailableTeams = teams.OrderBy(x => rnd.Next()).Take(2).ToList();

            var teamA = TwoRandomAvailableTeams.First();
            var teamB = TwoRandomAvailableTeams.Last();
            teamA.IsAvailable = false;
            teamB.IsAvailable = false;

            MatchTeam matchTeamA = new MatchTeam { Team = teamA, Seed = i };
            
            MatchTeam matchTeamB = new MatchTeam { Team = teamB, Seed = maxTeamCount };
            maxTeamCount--;

            gamematch.ParticipatingTeams.Add(matchTeamA);
            gamematch.ParticipatingTeams.Add(matchTeamB);
            gamematch.DateOfMatch = DateTimePicker.CreateRandomMatchTime();

            gamematches.Add(gamematch);
        }
        roundsNumber--;

        return gamematches;
    }

    //static League Generate(int playersNumber)
    //{
    //    // only works for power of 2 number of players   
    //    var roundsNumber = (int)Math.Log(playersNumber, 2);
    //    var rounds = roundsNumber;
    //    for (int i = 0; i < roundsNumber; i++)
    //    {
    //        var round = 1;
    //        var prevRound = i;
    //        if (prevRound == null)
    //        {
    //            // if first round - result is known
    //            round = new[] {
    //                new Match() {
    //                    PlayerA = 1,
    //                    PlayerB = 2
    //                }
    //            };
    //        }
    //        else
    //        {
    //            round.Matches = new Match[prevRound.Matches.Length * 2];
    //            // find median. For round 2 there are 4 players and median is 2.5 (between 2 and 3)
    //            // for round 3 there are 8 players and median is 4.5 (between 4 and 5)
    //            var median = (round.Matches.Length * 2 + 1) / 2f;
    //            var next = 0;
    //            foreach (var match in prevRound.Matches)
    //            {
    //                // you can play here by switching PlayerA and PlayerB or reordering stuff
    //                round.Matches[next] = new Match()
    //                {
    //                    PlayerA = match.PlayerA,
    //                    PlayerB = (int)(median + Math.Abs(match.PlayerA - median))
    //                };
    //                next++;
    //                round.Matches[next] = new Match()
    //                {
    //                    PlayerA = match.PlayerB,
    //                    PlayerB = (int)(median + Math.Abs(match.PlayerB - median))
    //                };
    //                next++;
    //            }
    //        }
    //        rounds[i] = round;
    //    }
    //    return rounds.Reverse().ToArray();
    //}


    private static int FindNumberOfRounds(int teamCount)
    {
        int output = 1;
        int val = 2;

        while (val < teamCount)
        {
            // output = output + 1;
            output += 1;

            // val = val * 2;
            val *= 2;
        }
        return output;
    }
    private static ICollection<LeagueTeam> RandomizeTeamOrder(List<LeagueTeam> teams)
    {
        return teams.OrderBy(x => Guid.NewGuid()).ToList();
        //return (teams.OrderBy(x => Guid.NewGuid())
        //    .Select(x => x.Team);
    }
}
