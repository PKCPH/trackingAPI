using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace trackingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : Controller
    {
        [HttpGet, Authorize]
        public IEnumerable<string> Get()
        {
            return new string[] { "John Doe", "Jane Doe" };
        }
    }
}
