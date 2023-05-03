using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using trackingAPI.Data;
using trackingAPI.Helpers;
using trackingAPI.Models;

namespace trackingAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TeamController : ControllerBase
{
    /// <summary>
    /// accessing the database at runtime
    /// </summary>
    private readonly DatabaseContext _context;
    private readonly IPlayerService _playerService;
    public TeamController(DatabaseContext context, IPlayerService playerService)
    {
        _context = context;
        _playerService = playerService;
    }

    //action method, does as a response of the http request, to get a list of Issue
    //attribute to make it handle httpGet!
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        //get a list of Issue
        var teams = await _context.Teams.ToListAsync();
        var playerTeams = await _context.PlayerTeams.ToListAsync();
        foreach (var team in teams)
        {
            foreach (var playerTeam in playerTeams)
            {
                if (playerTeam.TeamId == team.Id)
                {
                    team.Players.Add(playerTeam);
                }
            }
        }
        //if issue is not found return NotFound() (404 status) if found return Ok(issue) (200 status);
        return Ok(teams);
    }

    //handles the http request with the id at the end of the url: f.x. api/issue/*Id-Number*
    //so the action responds only to this id in the url
    //ProducesResponseType specifies which kind of status code the return can return
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(Team), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
    {
        //finding Issue with the id
        var team = await _context.Teams.FindAsync(id);
        var playerTeams = await _context.PlayerTeams.ToListAsync();
        foreach (var playerTeam in playerTeams)
        {
            if (playerTeam.TeamId == team.Id)
            {
                team.Players.Add(playerTeam);
            }
        }
        //if issue is not found return NotFound() (404 status) if found return Ok(issue) (200 status);
        return team == null ? NotFound() : Ok(team);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    //for creating a new issue
    public async Task<IActionResult> Create(Team team)
    {

        //adding the issue submitted by the request
        await _context.Teams.AddAsync(team);


        //saving the changes in the DB
        await _context.SaveChangesAsync();

        //returns the response with statuscode and a location in the editor
        return CreatedAtAction(nameof(GetById), new { id = team.Id }, team);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    //id is cound to the url, issue is bound to the body of the request
    public async Task<IActionResult> Update(Guid id, Team team)
    {
        //if the id of the url and the id in the body does not match, then return
        if (id != team.Id) return BadRequest();

        //otherwise we update the issue, save changes and
        _context.Entry(team).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id)
    {
        //finding the issue
        var teamToDelete = await _context.Teams.FindAsync(id);
        //if issues does not exist
        if (teamToDelete == null) return NotFound();

        //otherwise remove the issue and save changes in DB
        _context.Teams.Remove(teamToDelete);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    //get players from a specific team
    [HttpGet("players/{id}")]
    public async Task<IActionResult> GetPlayersFromTeam(Guid id)
    {
        var players = await _context.Players.ToListAsync();
        var playerTeams = await _context.PlayerTeams.ToListAsync();

        playerTeams.RemoveAll(s => s.TeamId != id);

        players.RemoveAll(p => !playerTeams.Exists(s => s.PlayerId == p.Id));
        
        return Ok(players);
    }

    [HttpPost("players/add")]
    public async Task<IActionResult> ChangePlayersOnTeam(List<List<PlayerTeam>> playerTeamsList)
        {
        if (playerTeamsList != null) await _playerService.ChangePlayersOnTeam(playerTeamsList);

        return playerTeamsList == null ? NotFound() : Ok(playerTeamsList);
    }

    //For creating league
    [HttpGet("getAvailableTeams")]
    public async Task<IActionResult> GetAvailableTeams()
    {
        //get a list of Issue
        var teams = await _context.Teams.Where(t => t.IsAvailable == true).OrderBy(t => t.Name).ToListAsync();
        var playerTeams = await _context.PlayerTeams.ToListAsync();
        foreach (var team in teams)
        {
            foreach (var playerTeam in playerTeams)
            {
                if (playerTeam.TeamId == team.Id)
                {
                    team.Players.Add(playerTeam);
                }
            }
        }
        //if issue is not found return NotFound() (404 status) if found return Ok(issue) (200 status);
        return Ok(teams);
    }
}