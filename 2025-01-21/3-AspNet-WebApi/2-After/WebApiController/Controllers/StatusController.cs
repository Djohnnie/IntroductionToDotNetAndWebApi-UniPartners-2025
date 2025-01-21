using Microsoft.AspNetCore.Mvc;

namespace WebApiController.Controllers;

[ApiController]
[Route("[controller]")]
public class StatusController : ControllerBase
{
    private readonly ILogger<StatusController> _logger;

    public StatusController(ILogger<StatusController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "GeefStatus")]
    public IActionResult Get()
    {
        _logger.LogInformation("Status opgevraagd");
        return Ok();
    }
}