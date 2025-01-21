using Microsoft.AspNetCore.Mvc;

namespace WebApiController.Controllers;

[ApiController]
[Route("[controller]")]
public class SomController : ControllerBase
{
    private readonly ILogger<SomController> _logger;

    public SomController(ILogger<SomController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "BerekenSom")]
    public IActionResult Get(int getal1, int getal2)
    {
        _logger.LogInformation($"Som berekend van {getal1} en {getal2}");
        return Ok(getal1 + getal2);
    }
}