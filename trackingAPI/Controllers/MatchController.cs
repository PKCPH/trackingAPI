using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using trackingAPI.Data;
using trackingAPI.Helpers;
using trackingAPI.Models;

namespace trackingAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MatchController : ControllerBase
{
    /// <summary>
    /// accessing the database at runtime
    /// </summary>
    private readonly DatabaseContext _context;

    public MatchController(DatabaseContext context) => _context = context;

    //action method, does as a response of the http request, to get a list of Issue
    //attribute to make it handle httpGet
    [HttpGet]
    public async Task<IEnumerable<GameMatch>> Get()
        //get a list of Issue
        => await _context.Matches.ToListAsync();

    //handles the http request with the id at the end of the url: f.x. api/issue/*Id-Number*
    //so the action responds only to this id in the url
    //ProducesResponseType specifies which kind of status code the return can return
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(GameMatch), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
    {
        //finding Issue with the id
        var match = await _context.Matches.FindAsync(id);
        //if issue is not found return NotFound() (404 status) if found return Ok(issue) (200 status);
        return match == null ? NotFound() : Ok(match);
    }

    [HttpPost("CreateOneMatch")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    //for creating a new issue
    public async Task<IActionResult> Create(TeamPicker teamPicker)
    {
        //adding the issue submitted by the request
        await _context.Matches.AddAsync(teamPicker.CreateMatch(_context));
        //saving the changes in the DB
        await _context.SaveChangesAsync();
        //handles the http request with the id at the end of the url: f.x. api/issue/*Id-Number*
        //so the action responds only to this id in the url
        //ProducesResponseType specifies which kind of status code the return can return
        return Ok(_context.Matches);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(Guid id, GameMatch match)
    {
        //if the id of the url and the id in the body does not match, then return
        if (id != match.Id) return BadRequest();

        //otherwise we update the issue, save changes and
        _context.Entry(match).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id)
    {
        //finding the issue
        var issueToDelete = await _context.Matches.FindAsync(id);
        //if issues does not exist
        if (issueToDelete == null) return NotFound();

        //otherwise remove the issue and save changes in DB!
        _context.Matches.Remove(issueToDelete);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpGet("/api/Matches")]
    public async Task<ActionResult<IList<GameMatch>>> GetAllMatchesAsync()
    {
        var matches = _context.Matches
            .Include(mt => mt.ParticipatingTeams)
            .ThenInclude(t => t.Team)
            .Select(match => new {
                Id = match.Id,
                dateOfMatch = match.DateOfMatch,
                matchState = match.MatchState,
                participatingTeams = match.ParticipatingTeams.Select(pt => new {
                    Id = pt.Team.Id,
                    name = pt.Team.Name,
                    result= pt.Result,
                }).ToList()
            })
            .ToList();

        return Ok(matches);
    }

    [HttpGet("/api/MatchDetails/{id}")]
    public async Task<ActionResult<IList<GameMatch>>> GetMatchDetails(Guid id)
    {
        var match = await _context.Matches
               .Include(mt => mt.ParticipatingTeams)
               .ThenInclude(t => t.Team)
               .Where(m => m.Id == id)
               .Select(m => new {
                   Id = m.Id,
                   dateOfMatch = m.DateOfMatch,
                   teamAScore = m.TeamAScore,
                   teamBScore = m.TeamBScore,
                   matchState = m.MatchState,
                   participatingTeams = m.ParticipatingTeams.Select(pt => new {
                       Id = pt.Team.Id,
                       name = pt.Team.Name
                   }).ToList()
               })
               .FirstOrDefaultAsync();

        if (match == null)
        {
            return NotFound();
        }

        return Ok(match);
    }


    /*    public ActionResult<IList<GameMatch>> GetAllMatches(int pageNumber = 1, int pageSize = 10)
        {
            var matches = _context.Matches
                .Include(mt => mt.ParticipatingTeams)
                .ThenInclude(t => t.Team)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return Ok(matches);
        }*/
}
