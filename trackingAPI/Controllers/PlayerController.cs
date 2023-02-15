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
            var player = await databaseContext.Players.FirstOrDefaultAsync(x => x.Id == id);
            if (player == null) { return NotFound(); }
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
            player.Team = updatePlayerRequest.Team;
            player.TeamId = updatePlayerRequest.TeamId;

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
