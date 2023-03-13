using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using trackingAPI.Data;
using trackingAPI.Helpers;
using trackingAPI.Models;

namespace trackingAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LeagueController : ControllerBase
{
    private readonly DatabaseContext _context;

    public LeagueController(DatabaseContext context) => _context = context;

  
    [HttpGet]
    public async Task<IEnumerable<League>> Get()
        => await _context.Leagues.ToListAsync();


    [HttpGet("{id}")]
    [ProducesResponseType(typeof(Team), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var league = await _context.Leagues.FindAsync(id);
        return league == null ? NotFound() : Ok(league);
    }

    //[HttpPost("{CreateOneLeagueAuto}")]
    //[ProducesResponseType(StatusCodes.Status201Created)]
    //public async Task<IActionResult> CreateAutoPick(League league)
    //{
    //    LeagueHelper leagueSeedingLogic = new LeagueHelper();
    //    await _context.Leagues.AddAsync(leagueSeedingLogic.CreateRounds(league, _context));
    //    await _context.SaveChangesAsync();
    //    return Ok(_context.Leagues);
    //}

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> Create(League league)
    {
        LeagueHelper leagueSeedingLogic = new LeagueHelper();
        await _context.Leagues.AddAsync(leagueSeedingLogic.CreateRounds(league, _context));
        await _context.SaveChangesAsync();
        return Ok(_context.Leagues);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(Guid id, League league)
    {
        if (id != league.Id) return BadRequest();
        _context.Entry(league).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var league = await _context.Leagues.FindAsync(id);
        if (league == null) return NotFound();
        _context.Leagues.Remove(league);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
