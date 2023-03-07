﻿using trackingAPI.Models;

namespace trackingAPI.Helpers;

public class LeagueHelpers
{
    //Order list of teams randomly
    //check if list is big enough, if not add byes - 2*2*2*2 - 2^4
    //create first round of matchups
    //create the rest of the rounds
    public static void CreateRounds(League league)
    {
        List<Team> randomizedTeams = RandomizeTeamOrder((List<Team>)league.Teams);
        int rounds = FindNumberOfRounds(randomizedTeams.Count);
        int byes = NumberOfByes(rounds, randomizedTeams.Count);

        //adding the first round to league
        league.Rounds.Add(CreateFirstRound(byes, randomizedTeams));

        CreateOtherRounds(league, rounds);
    }

    private static void CreateOtherRounds(League league, int rounds)
    {
        int round = 2;
        List<MatchupModel> previousRound = league.Rounds[0];
        List<MatchupModel> currentRound = new List<MatchupModel>();
        MatchupModel currentMatchup = new MatchupModel();

        while (round <= rounds)
        {
            //adding the current round
            foreach (MatchupModel match in previousRound)
            {
                currentMatchup.Entries.Add(new MatchupEntryModel { ParentMatchup = match });
                if(currentMatchup.Entries.Count > 1)
                {
                    currentMatchup.MatchupRound = round;
                    currentRound.Add(currentMatchup);
                    currentMatchup = new MatchupModel();
                }
            }

            //adding every entry to league.rounds and setting previousRound
            league.Rounds.Add(currentRound);
            previousRound = currentRound;

            currentRound = new List<MatchupModel>();
            round += 1;
        }
    } 

    private static List<MatchupModel> CreateFirstRound(int byes, List<Team> teams)
    {
        //we are adding bye to the first teams
        List<MatchupModel> output = new List<MatchupModel>();
        MatchupModel curr = new();

        foreach (Team team in teams)
        {
            //add entry
            curr.Entries.Add(new MatchupEntryModel { TeamCompeting = team });
            
            //if no byes or entries has more than 1
            if(byes > 0 || curr.Entries.Count > 1)
            {
                //create one round, add and 
                curr.MatchupRound = 1;
                output.Add(curr);
                //reusing curr with a new instance
                curr = new();
                
                //updating bye counter
                if(byes > 0)
                {
                    byes -= 1;
                }
            }
        }
        return output;
    }

    private static int NumberOfByes(int rounds, int numberOfTeams)
    {
        int output = 0;
        int totalTeams = 1;

        for (int i = 1; i < rounds; i++)
        {
            totalTeams *= 2;
        }
        output = totalTeams - numberOfTeams;
        return output;
    }

    private static int FindNumberOfRounds(int teamCount)
    {
        int output = 1;
        int val = 2;

        while(val < teamCount)
        {
            // output = output + 1;
            output += 1;

            // val = val * 2;
            val *= 2;
        }
        return output;
    }
    private static List<Team> RandomizeTeamOrder(List<Team> teams)
    {
        return teams.OrderBy(x => Guid.NewGuid()).ToList();
    }
}
