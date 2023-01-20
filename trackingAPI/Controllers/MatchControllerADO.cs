﻿using Microsoft.AspNetCore.Mvc;
using trackingAPI.Models;


namespace trackingAPI.Controllers
{
    public class MatchControllerADO : ControllerBase
    {

        [HttpPost]
        public async Task<IEnumerable<Match>> CreateMatch(Match match)
        {
            //choosing random team
            var choosenTeam = match.ParticipatingTeams.Count.ToString();
            ILogger _logger;

            //_logger.Log(LogLevel.Information, $"{choosenTeam}");

            //var conStr = DatabaseInit.ConnectionString;

            //// Creating Connection  
            //using (SqlConnection con = new SqlConnection(conStr))
                //{
                //    // Insert query  
                //    string query = "INSERT INTO MatchTeam(MatchesId,ParticipatingTeamsId) VALUES(@MatchesId, @ParticipatingTeamsId)";
                //    using (SqlCommand cmd = new SqlCommand(query))
                //    {
                //        cmd.Connection = con;
                //        // opening connection  
                //        con.Open();
                //        // Passing parameter values  
                //        cmd.Parameters.AddWithValue("@MatchesId", match.Id);
                //        cmd.Parameters.AddWithValue("@ParticipatingTeamsId", match.ParticipatingTeams.Where(pt => pt.IsAvailable == true && pt.Id));
                //        // Executing insert query  
                //        cmd.ExecuteNonQuery();

                //    }

                //}

            return (IEnumerable<Match>)Content(choosenTeam.ToString());
        }
    }
}



