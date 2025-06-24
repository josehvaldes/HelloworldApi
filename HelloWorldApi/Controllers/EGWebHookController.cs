using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Azure.Messaging.EventGrid;
using Microsoft.Extensions.Logging;
using Azure.Messaging.EventGrid.SystemEvents;
using System.Reflection.Metadata.Ecma335;

namespace HelloWorldApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EGWebHookController : ControllerBase
    {
        private readonly ILogger<EGWebHookController> _logger;
        public EGWebHookController(ILogger<EGWebHookController> logger)
        {
            _logger = logger;
        }

        [HttpPost("events")]
        public IActionResult Post([FromBody] JsonElement eventData)
        {
            _logger.LogInformation("Received event data: {EventData}", eventData);

            BinaryData binaryData = BinaryData.FromObjectAsJson(eventData);

            var eventGridEvents = EventGridEvent.ParseMany(binaryData);

            foreach (var eventGridEvent in eventGridEvents)
            {
                if (eventGridEvent.EventType == "Microsoft.EventGrid.SubscriptionValidationEvent")
                {
                    // Handle subscription validation
                    var data = eventGridEvent.Data.ToObjectFromJson<SubscriptionValidationEventData>();
                    var response = new
                    {
                        validationResponse = data.ValidationCode
                    };
                    var json = Results.Json(response);
                    return Ok(json);
                }
                else
                {
                    // Handle other events
                    Console.WriteLine($"Received event: {eventGridEvent.EventType}");
                    Console.WriteLine($"Subject: {eventGridEvent.Subject}");
                    Console.WriteLine($"Data: {eventGridEvent.Data}");
                }
            }

            return Ok();
        }
         
    }
}
