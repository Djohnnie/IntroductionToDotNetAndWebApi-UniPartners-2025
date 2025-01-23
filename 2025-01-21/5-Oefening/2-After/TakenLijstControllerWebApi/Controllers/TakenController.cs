using Microsoft.AspNetCore.Mvc;
using TakenLijstControllerWebApi.Beheer;
using TakenLijstControllerWebApi.Model;

namespace TakenLijstControllerWebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class TakenController : ControllerBase
{
    private readonly TakenBeheer _takenBeheer;

    public TakenController(TakenBeheer takenBeheer)
    {
        _takenBeheer = takenBeheer;
    }

    /// <summary>
    /// Endpoint voor het ophalen van alle taken.
    /// </summary>
    [HttpGet(Name = "GeefAlleTaken")]
    public IActionResult GeefAlleTaken()
    {
        var taken = _takenBeheer.GeefAlleTaken();

        return Ok(taken);
    }

    /// <summary>
    /// Endpoint voor het ophalen van een specifieke taak.
    /// </summary>
    [HttpGet("{code}", Name = "GeefTaak")]
    public IActionResult GeefTaak(Guid code)
    {
        var taak = _takenBeheer.GeefTaak(code);

        return taak is not null ? Ok(taak) : NotFound();
    }

    /// <summary>
    /// Endpoint voor het toevoegen van een nieuwe taak.
    /// </summary>
    [HttpPost(Name = "VoegTaakToe")]
    public IActionResult VoegTaakToe(Taak taak)
    {
        var nieuweTaak = _takenBeheer.VoegTaakToe(taak);

        return Created($"/taken/{nieuweTaak.Code}", nieuweTaak);
    }

    /// <summary>
    /// Endpoint voor het wijzigen van een bestaande taak.
    /// </summary>
    [HttpPut("{code}", Name = "WijzigTaak")]
    public IActionResult WijzigTaak(Guid code, Taak taak)
    {
        taak = taak with { Code = code };

        var gewijzigdeTaak = _takenBeheer.WijzigTaak(taak);

        return gewijzigdeTaak is not null ? Ok(gewijzigdeTaak) : NotFound();
    }

    /// <summary>
    /// Endpoint voor het voltooien van een taak.
    /// </summary>
    [HttpPut("{code}/voltooi", Name = "VoltooiTaak")]
    public IActionResult VoltooiTaak(Guid code)
    {
        var voltooideTaak = _takenBeheer.VoltooiTaak(code);

        return voltooideTaak is not null ? Ok(voltooideTaak) : NotFound();
    }

    /// <summary>
    /// Endpoint voor het verwijderen van een taak.
    /// </summary>
    [HttpDelete("{code}", Name = "VerwijderTaak")]
    public IActionResult VerwijderTaak(Guid code)
    {
        var taak = _takenBeheer.VerwijderTaak(code);

        return taak is not null ? Ok(taak) : NotFound();
    }
}