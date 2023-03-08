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
        Console.WriteLine("#######################" + bet.MatchId + "##############################");
        // Retrieve the match that the user is betting on
        var match = await _context.Matches.FindAsync(bet.MatchId);

        if (match == null)
        {
            // The specified match does not exist
            return StatusCode(410, "Invalid match ID");
        }

        if (match.MatchState == MatchState.Finished)
        {
            // The match has already finished (Maybe even look into bets not being able to be placed if match is ongoing
            return StatusCode(411, "Match has already finished");
        }

        if (bet.Amount <= 0)
        {
            // The bet amount is invalid
            return StatusCode(412, "Invalid bet amount");
        }

        var user = await _context.Logins.FindAsync(bet.LoginId);

        if (user == null)
        {
            // The specified user does not exist
            return StatusCode(413, "Invalid user ID");
        }

        if (user.Balance < bet.Amount)
        {
            // The user does not have sufficient funds
            return StatusCode(414, "Insufficient funds");
        }

        // Update the user's balance
        user.Balance -= bet.Amount;
        bet.Match = match;

        // Add the bet to the database
        _context.Bets.Add(bet);

        // Save changes to the database
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetBet), new { id = bet.Id }, bet);
    }

    [HttpGet("matchbets/{matchId}")]
    public async Task<ActionResult<IEnumerable<Bet>>> GetBetsForMatch(Guid matchId)
    {
        return await _context.Bets.Where(b => b.Match.Id == matchId).ToListAsync();
    }

    [HttpGet("mybets/{userId}")]
    public async Task<ActionResult<IList<Bet>>> GetBetsForUser(Guid userId)
    {
        /*return await _context.Bets.Include(mt => mt.Match).ThenInclude(t => t.ParticipatingTeams).ThenInclude(t => t.Team).Where(b => b.LoginId == userId).ToListAsync();*/

        var bets = _context.Bets
              .Include(mt => mt.Match)
              .ThenInclude(t => t.ParticipatingTeams)
              .ThenInclude(t => t.Team)
              .Where(m => m.LoginId == userId)
              .Select(m => new {
                  Id = m.Id,
                  MatchId = m.MatchId,
                  BetState = m.BetState,
                  BetResult = m.BetResult,
                  Team = m.Team,
                  Amount = m.Amount,
                  participatingTeams = m.Match.ParticipatingTeams.Select(pt => new {
                      Id = pt.Team.Id,
                      name = pt.Team.Name,
                      result = pt.Result,
                      score = pt.TeamScore
                  }).ToList()
              })
              .ToList();

        if (bets == null)
        {
            return NotFound();
        }

        return Ok(bets);
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

    [HttpGet]
    public async Task<IEnumerable<Bet>> Get()
    //get a list of Issue
    => await _context.Bets.ToListAsync();
}

