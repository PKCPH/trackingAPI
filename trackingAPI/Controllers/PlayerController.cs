using Microsoft.AspNetCore.Mvc;
using trackingAPI.Data;
using trackingAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace trackingAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlayerController : Controller
    {
        private readonly DatabaseContext databaseContext;

        public PlayerController(DatabaseContext databaseContext)
        {
            this.databaseContext = databaseContext;
        }

        //Read All
        [HttpGet]
        public async Task<IActionResult> GetAllPlayers()
        {
            var players = await databaseContext.Players.ToListAsync();
            var teams = await databaseContext.Teams.ToListAsync();
            //Goes through the list of teams and matches each player with the team they play on
            //foreach (var player in players)
            //{
            //    foreach (var team in teams)
            //    {
            //        if (team.Id == player.TeamId)
            //        {
            //            player.Team = team;
            //            //break not neccesary, but it should change O from n to n/2
            //            break;
            //        }
            //    }
            //}
            return Ok(players);
        }

        //Create
        [HttpPost]
        public async Task<IActionResult> AddPlayer([FromBody] Player playerRequest)
        {
            playerRequest.Id = Guid.NewGuid();
            await this.databaseContext.Players.AddAsync(playerRequest);
            await this.databaseContext.SaveChangesAsync();

            return Ok(playerRequest);
        }

        //Read one
        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetPlayer([FromRoute] Guid id)
        {
            //gets the player by looking up the player table for ID matches
            var player = await databaseContext.Players.FirstOrDefaultAsync(x => x.Id == id);
            if (player == null ) { return NotFound(); }
            //gets the team by looking up the team table for ID matches to the players teamID property
            //var team = await databaseContext.Teams.FirstOrDefaultAsync(x => x.Id == player.TeamId);
            //if (team == null) { return NotFound(); }
            //player.Team = team;
            return Ok(player);
        }

        //Update
        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> UpdatePlayer([FromRoute] Guid id, Player updatePlayerRequest)
        {
            var player = await this.databaseContext.Players.FindAsync(id);
            if (player == null) { return NotFound(); }
            player.Name = updatePlayerRequest.Name;
            player.Age = updatePlayerRequest.Age;
            //player.TeamId = updatePlayerRequest.TeamId;

            await this.databaseContext.SaveChangesAsync();
            return Ok(player);
        }

        //Delete
        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> DeletePlayer([FromRoute] Guid id)
        {
            var player = await this.databaseContext.Players.FindAsync(id);
            if (player == null) { return NotFound(); }

            this.databaseContext.Players.Remove(player);
            await this.databaseContext.SaveChangesAsync();
            return Ok(player);
        }
    }
}
