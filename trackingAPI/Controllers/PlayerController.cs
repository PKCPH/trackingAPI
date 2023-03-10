using Microsoft.AspNetCore.Mvc;
using trackingAPI.Data;
using trackingAPI.Models;
using Microsoft.EntityFrameworkCore;
using trackingAPI.Helpers;

namespace trackingAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlayerController : Controller
    {
        private readonly DatabaseContext databaseContext;
        private readonly IPlayerService playerService;

        public PlayerController(DatabaseContext databaseContext, IPlayerService playerService)
        {
            this.databaseContext = databaseContext;
            this.playerService = playerService;
        }

        //Read All
        [HttpGet]
        public async Task<IActionResult> GetAllPlayers()
        {
            var players = await databaseContext.Players.ToListAsync();
            var playerTeams = await databaseContext.PlayerTeams.ToListAsync();

            var playerList = playerService.ReadAllPlayers(players, playerTeams);

            return Ok(playerList);
        }

        //Create
        [HttpPost]
        public async Task<IActionResult> AddPlayer([FromBody] Player playerRequest)
        {
            if (playerRequest != null) 
            {
                await playerService.CreatePlayer(playerRequest);
            }
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
            player = playerService.AssignTeamsToPlayer(player, playerTeams);
            return Ok(player);
        }

        //Update
        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> UpdatePlayer([FromRoute] Guid id, Player updatePlayerRequest)
        {
            var playerTeams = await databaseContext.PlayerTeams.ToListAsync();

            foreach ( var playerTeam in playerTeams)
            {
                if (playerTeam.PlayerId == updatePlayerRequest.Id)
                {
                    if(updatePlayerRequest.Teams.ToList().Exists(t => t.TeamId == playerTeam.TeamId) == false)
                    {
                        this.databaseContext.PlayerTeams.Remove(playerTeam);
                    }
                }
            }

            foreach (var team in updatePlayerRequest.Teams)
            {
                if (playerTeams.Exists(p => p.PlayerId == team.PlayerId && p.TeamId == team.TeamId) == false)
                {
                    team.Id = Guid.NewGuid();
                    await this.databaseContext.PlayerTeams.AddAsync(team);
                }
            }

            await this.databaseContext.SaveChangesAsync();
            return Ok(updatePlayerRequest);
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
