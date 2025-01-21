using Microsoft.AspNetCore.Mvc;
using WebApiController.Generators;
using WebApiController.Model;

namespace WebApiController.Controllers;

[ApiController]
[Route("[controller]")]
public class PersoonController : ControllerBase
{
    private readonly PersoonGenerator _persoonGenerator;
    private readonly ILogger<PersoonController> _logger;

    public PersoonController(
        PersoonGenerator persoonGenerator,
        ILogger<PersoonController> logger)
    {
        _persoonGenerator = persoonGenerator;
        _logger = logger;
    }

    [HttpGet(Name = "GenereerPersoon")]
    public IActionResult GenereerPersoon()
    {
        _logger.LogInformation("Persoon wordt gegenereerd");
        return Ok(_persoonGenerator.MaakPersoon());
    }

    [HttpPost("verjaardag", Name = "VierVerjaardag")]
    public IActionResult VierVerjaardag(Persoon persoon)
    {
        _logger.LogInformation($"Vier verjaardag van {persoon.Naam}");
        return Ok(persoon with { Leeftijd = persoon.Leeftijd + 1 });
    }
}