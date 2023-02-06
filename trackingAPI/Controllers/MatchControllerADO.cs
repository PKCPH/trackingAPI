using Microsoft.AspNetCore.Mvc;
using trackingAPI.Data;
using trackingAPI.Helpers;
using trackingAPI.Models;
using System.Data.SqlClient;


namespace trackingAPI.Controllers;

//[Route("api/[controller]")]
//[ApiController]
public class MatchControllerADO : ControllerBase
{

    [HttpPost]
    public async Task<IEnumerable<GameMatch>> CreateMatch(GameMatch match)
    {
        ////choosing random team
        //var choosenTeam = match.ParticipatingTeams.Count.ToString();
        ////ILogger _logger;

        TeamPicker teamPicker = new();
        

        //_logger.Log(LogLevel.Information, $"{choosenTeam}");

        var conStr = DatabaseInit.ConnectionString;

        // Creating Connection  
        using (SqlConnection con = new SqlConnection(conStr))
        {
            // Insert query  
            string query = "INSERT INTO MatchTeam(MatchesId,ParticipatingTeamsId) VALUES(@MatchesId, @ParticipatingTeamsId)";
            using (SqlCommand cmd = new SqlCommand(query))
            {
                cmd.Connection = con;
                // opening connection  
                con.Open();
                // Passing parameter values  
                cmd.Parameters.AddWithValue("@MatchesId", match.Id);
                //if(match.Id == match.ParticipatingTeams.Where())
                cmd.Parameters.AddWithValue("@ParticipatingTeamsId", match.ParticipatingTeams);
                // Executing insert query  
                cmd.ExecuteNonQuery();

            }

        }

        return (IEnumerable<GameMatch>)Content(match.ParticipatingTeams.ToString());
    }
}



