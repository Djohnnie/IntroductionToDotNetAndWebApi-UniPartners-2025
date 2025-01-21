# IntroductionToDotNetAndWebApi-UniPartners-2025
ASP.NET WebApi (Controllers, Actions, DI, Swagger)

## Een WebApi service maken

Maak een nieuw ASP.NET WebApi project aan:

```
dotnet new webapi -controllers --name WebApiController
```

Maak de gegenereerde template-code wat schoon totdat je dit overhoudt:

```csharp
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
```

Navigaar naar de 'WeatherForecastController', verwijder een deel van de automatisch gegenereerde code en hernoem de klasse en het bestand naar de 'StatusController':

```csharp
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
```

Voeg opnieuw een 'rest.http' bestand toe aan de root van je project om je endpoints te testen:

```py
@baseUrl = http://localhost:5209

### GeefStatus
GET {{baseUrl}}/status
```

Voeg een nieuwe controller 'SomController' toe waarin een som kan berekend worden:

```csharp
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
```

En voeg een nieuwe test toe om deze nieuwe endpoint te testen:

```py
### BerekenSom
@getal1 = 5
@getal2 = 3
GET {{baseUrl}}/som?getal1={{getal1}}&getal2={{getal2}}
```

Voeg een nieuwe controller 'PersoonController' toe om de endpoints rond personen te bundelen:

```csharp
using Microsoft.AspNetCore.Mvc;
using WebApiController.Generators;
using WebApiController.Model;

namespace WebApiController.Controllers;

[ApiController]
[Route("[controller]")]
public class PersoonController : ControllerBase
{
    private readonly ILogger<PersoonController> _logger;

    public PersoonController(ILogger<PersoonController> logger)
    {
        _logger = logger;
    }
}
```

Maak een nieuwe map 'Model' onder de root van het project en maak daarin een nieuw record 'Persoon':

```csharp
namespace WebApiController.Model;

public record Persoon(string Naam, int Leeftijd);
```

Maak een nieuwe map 'Generators' onder de root van het project en maak daarin een drie nieuwe klassen om een Persoon, Naam en Leeftijd te genereren:

```csharp
namespace WebApiController.Generators;

public class NaamGenerator
{
    public string GenereerNaam()
    {
        return "Karel";
    }
}
```

```csharp
namespace WebApiController.Generators;

public class NummerGenerator
{
    public int GenereerNummer()
    {
        return 42;
    }
}
```

```csharp
using WebApiController.Model;

namespace WebApiController.Generators;

public class PersoonGenerator
{
    private readonly NaamGenerator _naamGenerator;
    private readonly NummerGenerator _nummerGenerator;

    public PersoonGenerator(NaamGenerator naamGenerator, NummerGenerator nummerGenerator)
    {
        _naamGenerator = naamGenerator;
        _nummerGenerator = nummerGenerator;
    }

    public Persoon MaakPersoon()
    {
        return new(_naamGenerator.GenereerNaam(), _nummerGenerator.GenereerNummer());
    }
}
```

Keer terug naar de PersoonController en voeg de twee endpoints toe rond 'verjaardag' en 'genereer persoon':

```csharp
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
```

Voeg als laatste ook nog de testen toe in het 'rest.http' bestand:

```py
### VierVerjaardag
POST {{baseUrl}}/persoon/verjaardag
Content-Type: application/json

{
    "naam": "Jozef",
    "leeftijd": 12
}

### GenereerPersoon
GET {{baseUrl}}/persoon
```