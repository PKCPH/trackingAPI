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

    //action method, does as a response of the http request, to get a list of Issue
    //attribute to make it handle httpGet
    [HttpGet]
    public async Task<IEnumerable<League>> Get()
        //get a list of Issue
        => await _context.Leagues.ToListAsync();


    [HttpGet("{id}")]
    [ProducesResponseType(typeof(Team), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
    {
        //finding Issue with the id
        var league = await _context.Leagues.FindAsync(id);
        //if issue is not found return NotFound() (404 status) if found return Ok(issue) (200 status);
        return league == null ? NotFound() : Ok(league);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    //for creating a new issue
    public async Task<IActionResult> Create(League league)
    {
        //LeagueSeedingLogic.CreateRounds(league);
        ////adding the issue submitted by the request
        await _context.Leagues.AddAsync(league);
        ////saving the changes in the DB
        await _context.SaveChangesAsync();
        //returns the response with statuscode and a location in the editor
        return Ok(_context.Leagues);
    }

    //[HttpPost]
    //[ProducesResponseType(StatusCodes.Status201Created)]
    ////for creating a new issue
    //public async Task<IActionResult> Create(LeagueSeedingHelper leagueSeedingHelper)
    //{
    //    //adding the issue submitted by the request
    //    await _context.Leagues.AddAsync(leagueSeedingHelper.SeedDistribution(_context));
    //    //saving the changes in the DB
    //    await _context.SaveChangesAsync();
    //    //returns the response with statuscode and a location in the editor
    //    return CreatedAtAction(nameof(GetById), new { id = league.Id }, league);
    //}

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(Guid id, League league)
    {
        //if the id of the url and the id in the body does not match, then return
        if (id != league.Id) return BadRequest();

        //otherwise we update the issue, save changes and
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
