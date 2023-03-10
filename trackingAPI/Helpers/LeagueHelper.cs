using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using trackingAPI.Data;
using trackingAPI.Models;

namespace trackingAPI.Helpers;

public class LeagueHelper
{
    public League CreateRounds(League league, DatabaseContext _context)
    {
        
        
        LeagueHelper leagueHelper = new LeagueHelper();
        var leagueTeams = leagueHelper.GetListOfEightTeams(league, _context);

        league.Teams = leagueTeams.Teams.ToList();
        league.StartDate = DateTime.Now;
        league.LeagueState = LeagueState.NotStarted;
        league.Name = "testleague123";

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

    public League GetListOfEightTeams(League league, DatabaseContext _context)
    {
        Random rnd = new Random();

        var eightAvailableTeams = _context.Teams.Where(t => (bool)t.IsAvailable).ToList().Take(16);

        foreach (var team in eightAvailableTeams)
        {
            LeagueTeam leagueTeamA = new LeagueTeam { Team = team };
            league.Teams.Add(leagueTeamA);
        }
        return league;
    }

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
