//using Microsoft.AspNetCore.Mvc;
//using trackingAPI.Data;
//using trackingAPI.Models;

//namespace NET6WebAPI.Controllers
//{
//    [ApiController]
//    [Route("api/[controller]")]
//    public class PlayersController : Controller
//    {
//        private readonly DatabaseContext _context;
//        public PlayersController(DatabaseContext context) => _context = context;

//        [HttpGet]
//        public async Task<IActionResult> GetAllPlayers()
//        {
//            var players = await _context.Player.ToList();

//            return Ok(players);
//        }

//        [HttpPost]

//        public async Task<IActionResult> InsertPlayer([FromBody] Player playerUserValues)
//        {
//            playerUserValues.Id = Guid.NewGuid();
//            await _angularDbContext.Players.AddAsync(playerUserValues);
//            await _angularDbContext.SaveChangesAsync();

//            return Ok(playerUserValues);
//        }

//        [HttpGet]
//        [Route("{id:Guid}")]
//        public async Task<IActionResult> GetPlayer([FromRoute] Guid id)
//        {
//            var player = await _angularDbContext.Players.FirstOrDefaultAsync(x => x.Id == id);

//            if (player == null) { return NotFound(); }
//            else return Ok(player);
//        }

//        [HttpPut]
//        [Route("{id:Guid}")]

//        public async Task<IActionResult> UpdatePlayer([FromRoute] Guid id, Player updatePlayer)
//        {
//            var player = await _angularDbContext.Players.FindAsync(id);

//            if (player == null)
//            {
//                return NotFound();
//            }
//            else
//            {
//                player.Name = updatePlayer.Name;
//                player.Age = updatePlayer.Age;
//                player.Salary = updatePlayer.Salary;
//                player.Team = updatePlayer.Team;
//                player.Position = updatePlayer.Position;
//                player.Salary = updatePlayer.Salary;

//                await _angularDbContext.SaveChangesAsync();

//                return Ok(player);
//            }

//        }

//        [HttpDelete]
//        [Route("{id:Guid}")]

//        public async Task<IActionResult> DeletePlayer([FromRoute] Guid id)
//        {
//            var player = await _angularDbContext.Players.FindAsync(id);
//            if (player == null)
//            {
//                return NotFound();
//            }
//            else
//            {
//                _angularDbContext.Players.Remove(player);
//                await _angularDbContext.SaveChangesAsync();

//                return Ok(player);
//            }
//        }

//    }
//}
