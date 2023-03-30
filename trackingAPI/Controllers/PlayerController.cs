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
            var player = await databaseContext.Players.FindAsync(id);
            if(player == null ) { return NotFound(); }

            await playerService.PlayerUpdateRemovePlayerTeams(updatePlayerRequest, playerTeams);

            await playerService.PlayerUpdateAddPlayerTeams(updatePlayerRequest, playerTeams);
            
            player.Id = updatePlayerRequest.Id;
            player.Name= updatePlayerRequest.Name;
            player.Age = updatePlayerRequest.Age;
            player.Overall = updatePlayerRequest.Overall;
            player.Potential = updatePlayerRequest.Potential;
            player.Pace = updatePlayerRequest.Pace;
            player.Shooting = updatePlayerRequest.Shooting;
            player.Passing = updatePlayerRequest.Passing;
            player.Dribbling = updatePlayerRequest.Dribbling;
            player.Defense = updatePlayerRequest.Defense;
            player.Physical = updatePlayerRequest.Physical;

            await databaseContext.SaveChangesAsync();

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

            await this.databaseContext.SaveChangesAsync();
            return Ok(player);
        }

        //Read Limited rows
        [HttpGet]
        [Route("{rowSkip:int}-{numberOfRows:int}-{name}-{nationality}-{younger:bool}-{age:int}-{shorter:bool}-{height:int}-{lighter:bool}-{weight:int}-{worse:bool}-{overall:int}-{position}-{preferred_foot}")]
        public async Task<IActionResult> GetLimitedPlayers(
            [FromRoute] int rowSkip, 
            [FromRoute] int numberOfRows, 
            [FromRoute] string? name = "", 
            [FromRoute] string nationality = "", 
            [FromRoute] bool younger = false, 
            [FromRoute] int age = 0, 
            [FromRoute] bool shorter = false, 
            [FromRoute] int height = 0, 
            [FromRoute] bool lighter = false, 
            [FromRoute] int weight = 0, 
            [FromRoute] bool worse = false, 
            [FromRoute] int overall = 0, 
            [FromRoute] string position = "", 
            [FromRoute] string preferred_foot = "")
        {
            var players = await databaseContext.Players.Skip(rowSkip).Take(numberOfRows).Where(p => 
                p.Name.ToLower().Contains(name.ToLower()) && 
                p.nationality.ToLower().Contains(nationality.ToLower()) && 
                younger ? p.Age <= age : p.Age >= age && 
                shorter ? p.height_cm <= height : p.height_cm >= height && 
                lighter ? p.weight_kg <= weight : p.weight_kg >= weight && 
                worse ? p.Overall <= overall : p.Overall >= overall && 
                p.player_positions.ToLower().Contains(position.ToLower()) && 
                p.preferred_foot.ToLower().Contains(preferred_foot.ToLower())
            ).ToListAsync();

            var playerTeams = await databaseContext.PlayerTeams.ToListAsync();
            var totalPlayers = await databaseContext.Players.ToListAsync();

            var playerList = playerService.ReadAllPlayers(players, playerTeams);
            var playerCount = totalPlayers.Count();

            List<object> Response = new List<object>
            {
                playerCount,
                playerList
            };

            return Ok(Response);
        }
    }
}