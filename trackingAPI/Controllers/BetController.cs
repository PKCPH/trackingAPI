using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using trackingAPI.Data;
using trackingAPI.Helpers;
using trackingAPI.Models;

namespace trackingAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BetController : ControllerBase
{
    /// <summary>
    /// accessing the database at runtime
    /// </summary>
    private readonly DatabaseContext _context;

    public BetController(DatabaseContext context) => _context = context;

    //[HttpPost]
    //public async Task<ActionResult<Bet>> PlaceBet(Bet bet)
    //{
    //    _context.Bets.Add(bet);
    //    await _context.SaveChangesAsync();

    //    return CreatedAtAction(nameof(GetBet), new { id = bet.Id }, bet);
    //}

    [HttpPost("place")]
    public async Task<ActionResult<Bet>> PlaceBet(Bet bet)
    {
        // Retrieve the match that the user is betting on
        var match = await _context.Matches.FindAsync(bet.GameMatchId);

        if (match == null)
        {
            // The specified match does not exist
            return BadRequest("Invalid match ID");
        }

        if (match.MatchState == MatchState.Finished)
        {
            // The match has already finished (Maybe even look into bets not being able to be placed if match is ongoing
            return BadRequest("Match has already finished");
        }

        if (bet.Amount <= 0)
        {
            // The bet amount is invalid
            return BadRequest("Invalid bet amount");
        }

        var user = await _context.Logins.FindAsync(bet.LoginId);

        if (user == null)
        {
            // The specified user does not exist
            return BadRequest("Invalid user ID");
        }

        if (user.Balance < bet.Amount)
        {
            // The user does not have sufficient funds
            return BadRequest("Insufficient funds");
        }

        // Update the user's balance
        user.Balance -= bet.Amount;

        // Add the bet to the database
        _context.Bets.Add(bet);

        // Save changes to the database
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetBet), new { id = bet.Id }, bet);
    }

    [HttpGet("{matchId}")]
    public async Task<ActionResult<IEnumerable<Bet>>> GetBetsForMatch(Guid matchId)
    {
        return await _context.Bets.Where(b => b.GameMatchId == matchId).ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Bet>> GetBet(int id)
    {
        var bet = await _context.Bets.FindAsync(id);

        if (bet == null)
        {
            return NotFound();
        }

        return bet;
    }
}

