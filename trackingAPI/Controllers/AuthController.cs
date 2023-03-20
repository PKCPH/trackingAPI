using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Collections.Generic;
using trackingAPI.Data;
using trackingAPI.Helpers;
using trackingAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly DatabaseContext _context;
    private readonly ITokenService _tokenService;
    public AuthController(DatabaseContext context, ITokenService tokenService)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
    }
    [HttpPost, Route("login")]
    public IActionResult Login([FromBody] Login login)
    {
        if (login is null)
        {
            return BadRequest("Invalid client request");
        }

        var user = _context.Logins.FirstOrDefault(u =>
            (u.UserName == login.UserName) && (u.Password == login.Password));

        if (user is null)
            return Unauthorized();
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, login.UserName),
        };
        if (user.Role == "Admin")
        {
            claims.Add(new Claim(ClaimTypes.Role, "Admin"));
        }
        else
        {
            claims.Add(new Claim(ClaimTypes.Role, "User"));
        }
        var accessToken = _tokenService.GenerateAccessToken(claims);
        var refreshToken = _tokenService.GenerateRefreshToken();
        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);
        _context.SaveChanges();
        return Ok(new AuthenticatedResponse
        {
            Token = accessToken,
            RefreshToken = refreshToken
        });
    }

    //action method, does as a response of the http request, to get a list of Issue
    //attribute to make it handle httpGet
    [HttpGet, Authorize(Roles = "Admin")]
    public async Task<IEnumerable<Login>> Get()
        //get a list of Issue
        => await _context.Logins.ToListAsync();

    [HttpPost, Route("register")]
    public async Task<IActionResult> Create(Login login)
    {
        login.Role = "User";

        //adding the issue submitted by the request
        await _context.Logins.AddAsync(login);

        //saving the changes in the DB
        await _context.SaveChangesAsync();

        //returns the response with statuscode and a location in the editor
        return Ok(login);
    }

    [HttpGet("{username}")]
    [ProducesResponseType(typeof(Login), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByName(string username)
    {
        //finding Issue with the id
        var login = await _context.Logins.FirstOrDefaultAsync(x => x.UserName == username);
        //if issue is not found return NotFound() (404 status) if found return Ok(issue) (200 status);
        return login == null ? NotFound() : Ok(login);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    //id is cound to the url, issue is bound to the body of the request
    public async Task<IActionResult> Update(Guid id, Login login)
    {
        //if the id of the url and the id in the body does not match, then return
        if (id != login.Id) return BadRequest();

        //otherwise we update the issue, save changes and
        _context.Entry(login).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{username}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(string username)
    {
        //finding the issue
        var userToDelete = await _context.Logins.FirstOrDefaultAsync(x => x.UserName == username);

        //otherwise remove the issue and save changes in DB
        _context.Logins.Remove(userToDelete);
        await _context.SaveChangesAsync();

        return NoContent();
    }


    [HttpGet, Route("reset")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ResetMatchesAndTeams()
    {
        //finding the issue
        var matches = await _context.Matches.ToListAsync();

        if (matches == null || !matches.Any())
        {
            //if no entities found, return a 404 Not Found response
            return NotFound();
        }

        //otherwise remove the issue and save changes in DB
        _context.Matches.RemoveRange(matches);

        var teams = await _context.Teams.ToListAsync();

        if (teams == null || !teams.Any())
        {
            return NotFound();
        }

        foreach (var team in teams)
        {
            team.IsAvailable = true;
        }

        await _context.SaveChangesAsync();
        return NoContent();
    }
}