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

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] League league)
    {
        LeagueHelper leagueHelper = new LeagueHelper();
        await _context.Leagues.AddAsync(leagueHelper.CreateRounds(league, _context));
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

    [HttpGet("/api/Leagues")]
    public async Task<ActionResult<IList<League>>> GetLeagues()
    {
        /*return await _context.Bets.Include(mt => mt.Match).ThenInclude(t => t.ParticipatingTeams).ThenInclude(t => t.Team).Where(b => b.LoginId == userId).ToListAsync();*/

        var leagues = _context.Leagues
  /*            .Where(mt => mt.Gamematches.Any(t => t.MatchState != MatchState.Finished))*/
              .Include(mt => mt.Gamematches)
              .ThenInclude(t => t.ParticipatingTeams)
              .Select(m => new {
                  Id = m.Id,
                  StartDate = m.StartDate,
                  LeagueState = m.LeagueState,
                  Name = m.Name,
                  Match = m.Gamematches.Select(gm => new {
                      Id = gm.Id,
                      dateOfMatch = gm.DateOfMatch,
                      matchState = gm.MatchState,
                      round = gm.Round,
                      participatingTeams = gm.ParticipatingTeams.Select(pt => pt.Team != null ? (object)new
                      {
                          Id = pt.Team.Id,
                          name = pt.Team.Name,
                          result = pt.Result,
                          score = pt.TeamScore,
                      } : null).ToList()
                  }).ToList()
              })
              .ToList();

        if (leagues == null)
        {
            return NotFound();
        }


        return Ok(leagues);
    }

    [HttpPost]
    public async Task<IActionResult> AddTeamsToLeague([FromBody] League model)
    {
        // Create the league
        League league = new League
        {
            Name = model.Name,
            StartDate = model.StartDate
        };
        _context.Leagues.Add(league);
        await _context.SaveChangesAsync();

        // Obtain the newly created leagueId
        Guid leagueId = league.Id;

        // Loop through the teams and add them to the league
        foreach (var teamDto in model.Teams)
        {
            // Find the existing team by teamId
            Team team = await _context.Teams.FirstOrDefaultAsync(t => t.Id == teamDto.Team.Id);

            if (team != null)
            {
                // Create the LeagueTeam relationship
                LeagueTeam leagueTeam = new LeagueTeam
                {
                    Team = team,
                    League = league
                };
                _context.LeagueTeams.Add(leagueTeam);
            }
        }

        await _context.SaveChangesAsync();

        return Ok("Teams added to league successfully");
    }
}
