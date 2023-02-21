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
            var playerTeams = await databaseContext.PlayerTeams.ToListAsync();
            //Goes through the list of teams and matches each player with the team they play on
            foreach (var player in players)
            {
                foreach (var playerTeam in playerTeams)
                {
                    if (playerTeam.PlayerId == player.Id)
                    {
                        player.Teams.Add(playerTeam);
                        //break not neccesary, but it should change O from n to n/2
                        break;
                    }
                }
            }
            return Ok(players);
        }

        //Create
        [HttpPost]
        public async Task<IActionResult> AddPlayer([FromBody] Player playerRequest)
        {
            playerRequest.Id = Guid.NewGuid();
            foreach (var playerTeam in playerRequest.Teams)
            {
                playerTeam.PlayerId = playerRequest.Id;
                await this.databaseContext.PlayerTeams.AddAsync(playerTeam);
            }
            playerRequest.Teams.Clear();
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
            var player = await databaseContext.Players.FindAsync(id);
            if (player == null ) { return NotFound(); }
            //gets the team by looking up the team table for ID matches to the players teamID property
            var playerTeams = await databaseContext.PlayerTeams.ToListAsync();
            foreach ( var playerTeam in playerTeams)
            {
                if (player.Id == playerTeam.PlayerId)
                {
                    player.Teams.Add(playerTeam);
                }
            }
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
            player.age = updatePlayerRequest.age;

            await this.databaseContext.SaveChangesAsync();
            return Ok(player);
        }

        //Delete
        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> DeletePlayer([FromRoute] Guid id)
        {
            var player = await this.databaseContext.Players.FindAsync(id);
            var playerTeams = await databaseContext.PlayerTeams.ToListAsync();

            //removes the player from the player Table
            if (player != null)
            {
                this.databaseContext.Players.Remove(player);
            }

            //removes all playerteams with the playerId from the playerTeam Table
            playerTeams.RemoveAll(p => p.PlayerId == id);

            await this.databaseContext.SaveChangesAsync();
            return Ok(player);
        }
    }
}
