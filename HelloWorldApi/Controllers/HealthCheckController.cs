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
            _logger.LogCritical("> GetStatus critical Log with log");
            _logger.LogError("> GetStatus error Log with log");
            return Ok(new { status="Healthy with logs" } );
        }
    }
}
