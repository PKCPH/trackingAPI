using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using trackingAPI.Data;
using trackingAPI.Models;

namespace trackingAPI.Controllers;

//contains actions
[Route("api/[controller]")]
[ApiController]
public class IssueController : ControllerBase
{
    /// <summary>
    /// accessing the database at runtime
    /// </summary>
    private readonly IssueDBContext _context; 
    public IssueController(IssueDBContext context) => _context = context;

    //action method, does as a response of the http request, to get a list of Issue
    //attribute to make it handle httpGet
    [HttpGet]
    public async Task<IEnumerable<Issue>> Get()
        //get a list of Issue
        => await _context.Issues.ToListAsync();

    //handles the http request with the id at the end of the url: f.x. api/issue/*Id-Number*
    //so the action responds only to this id in the url
    //ProducesResponseType specifies which kind of status code the return can return
    [HttpGet("id")]
    [ProducesResponseType(typeof(Issue), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        //finding Issue with the id
        var issue = await _context.Issues.FindAsync(id);
        //if issue is not found return NotFound() (404 status) if found return Ok(issue) (200 status);
        return issue == null ? NotFound() : Ok(issue);
    }
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    //for creating a new issue
    public async Task<IActionResult> Create(Issue issue)
    {
        //adding the issue submitted by the request
        await _context.Issues.AddAsync(issue);
        //saving the changes in the DB
        await _context.SaveChangesAsync();

        //returns the response with statuscode and a location in the editor
        return CreatedAtAction(nameof(GetById), new {id = issue.Id}, issue);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    //id is cound to the url, issue is bound to the body of the request
    public async Task<IActionResult> Update(int id, Issue issue)
    {
        //if the id of the url and the id in the body does not match, then return
        if (id != issue.Id) return BadRequest();

        //otherwise we update the issue, save changes and
        _context.Entry(issue).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        //finding the issue
        var issueToDelete = await _context.Issues.FindAsync(id);
        //if issues does not exist
        if(issueToDelete == null) return NotFound();

        //otherwise remove the issue and save changes in DB
        _context.Issues.Remove(issueToDelete);
        await _context.SaveChangesAsync();

        return NoContent();
    }


}
