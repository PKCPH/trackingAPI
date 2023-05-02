using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using trackingAPI.Data;
using trackingAPI.Models;

namespace trackingAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TimelogController : ControllerBase
{
    /// <summary>
    /// accessing the database at runtime
    /// </summary>
    private readonly DatabaseContext _context;
    public TimelogController(DatabaseContext context) => _context = context;

    [HttpGet]
    public async Task<IEnumerable<Timelog>> Get()
       //get a list of Issue
       => await _context.Timelog.ToListAsync();
    
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(Timelog), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
    {
        //finding Issue with the id
        var timelog = await _context.Timelog.FindAsync(id);
        //if issue is not found return NotFound() (404 status) if found return Ok(issue) (200 status);
        return timelog == null ? NotFound() : Ok(timelog);
    }

    [HttpGet("timelog/{gamematchId}")]
    [ProducesResponseType(typeof(Timelog), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IEnumerable<Timelog>> GetByGamematchId(Guid gamematchId)
    {
        return await _context.Timelog.Where(t => t.GamematchId == gamematchId).ToListAsync();

        ////finding Issue with the id
        //var timelog = await _context.Timelog.FindAsync(id);
        ////if issue is not found return NotFound() (404 status) if found return Ok(issue) (200 status);
        //return timelog == null ? NotFound() : Ok(timelog);
    }

    [HttpGet("latestTimelog/{gamematchId}")]
    [ProducesResponseType(typeof(Timelog), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetLatestByGamematchId(Guid gamematchId)
    {
        var timelog = await _context.Timelog
            .Where(t => t.GamematchId == gamematchId)
            .Where(t => t.CategoryLog != CategoryLog.MatchEvent)
            .OrderBy(t => t.DateTime)
            .LastAsync();

        return timelog == null ? NotFound() : Ok(timelog);
    }
}
