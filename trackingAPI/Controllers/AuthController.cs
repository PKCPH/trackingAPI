using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Collections.Generic;
using trackingAPI.Data;
using trackingAPI.Helpers;
using trackingAPI.Models;
using Microsoft.EntityFrameworkCore;

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
}