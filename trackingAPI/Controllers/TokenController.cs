using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using trackingAPI.Helpers;
using trackingAPI.Models;
using trackingAPI.Data;

namespace trackingAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TokenController : ControllerBase
{
    private readonly DatabaseContext _context;
    private readonly ITokenService _tokenService;
    public TokenController(DatabaseContext context, ITokenService tokenService)
    {
        this._context = context ?? throw new ArgumentNullException(nameof(context));
        this._tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
    }
    [HttpPost]
    [Route("refresh")]
    public IActionResult Refresh(TokenApiModel tokenApiModel)
    {
        if (tokenApiModel is null)
            return BadRequest("Invalid client request");

        string accessToken = tokenApiModel.AccessToken;
        string refreshToken = tokenApiModel.RefreshToken;

        var principal = _tokenService.GetPrincipalFromExpiredToken(accessToken);
        var username = principal.Identity.Name; //this is mapped to the Name claim by default

        var user = _context.Logins.SingleOrDefault(u => u.UserName == username);

        if (user is null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
            return BadRequest("Invalid client request");

        var newAccessToken = _tokenService.GenerateAccessToken(principal.Claims);
        var newRefreshToken = _tokenService.GenerateRefreshToken();

        user.RefreshToken = newRefreshToken;
        _context.SaveChanges();

        return Ok(new AuthenticatedResponse()
        {
            Token = newAccessToken,
            RefreshToken = newRefreshToken
        });
    }

    [HttpPost, Authorize]
    [Route("revoke")]
    public IActionResult Revoke()
    {
        var username = User.Identity.Name;
        var user = _context.Logins.SingleOrDefault(u => u.UserName == username);
        if (user == null) return BadRequest();
        user.RefreshToken = null;
        _context.SaveChanges();
        return NoContent();
    }
}
