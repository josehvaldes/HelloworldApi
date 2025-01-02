using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HelloWorldApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HealthCheckController : ControllerBase
    {

        public HealthCheckController() { }

        [HttpGet("status")]
        public IActionResult GetStatus() 
        {
            return Ok(new { status="Healthy" } );
        }
    }
}
