using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using trackingAPI.Data;
using trackingAPI.Helpers.SignalRHelpers;
using trackingAPI.Hubs;
using trackingAPI.Models;

namespace trackingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChartController : ControllerBase
    {
        private readonly DatabaseContext _context;
        private readonly IHubContext<ChartHub> _hub;
        private readonly TimerManager _timer;

        public ChartController(IHubContext<ChartHub> hub, TimerManager timer, DatabaseContext context)
        {
            _hub = hub;
            _timer = timer;
            _context = context;
        }
        [HttpGet]
        public IActionResult Get()
        {
            var teams = _context.Teams.ToList(); // Retrieve the list of teams from the database

            if (!_timer.IsTimerStarted)
                _timer.PrepareTimer(() => _hub.Clients.All.SendAsync("TransferChartData", teams));

            return Ok(new { Message = "Request Completed" });
        }

    } 
}
