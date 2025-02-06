using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HelloWorldApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HealthCheckController : ControllerBase
    {
        private readonly ILogger<HealthCheckController> _logger;
        public HealthCheckController(ILogger<HealthCheckController> logger) 
        {
            _logger = logger;
        }

        [HttpGet("status")]
        public IActionResult GetStatus() 
        {
            _logger.LogCritical(">GetStatus Log");
            return Ok(new { status="Healthy" } );
        }
    }
}
