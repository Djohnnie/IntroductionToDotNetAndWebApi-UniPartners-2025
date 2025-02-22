var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
// Registreer het TakenBeheer in de Dependency Injection container als singleton.
// Op deze manier zal elke request dezelfde instantie van het TakenBeheer gebruiken
// en dus altijd toegang hebben tot de volledige lijst van taken.
builder.Services.AddSingleton<TakenBeheer>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// Endpoint voor het ophalen van alle taken.
app.MapGet("/taken", (TakenBeheer takenBeheer) =>
{
    var taken = takenBeheer.GeefAlleTaken();

    return Results.Ok(taken);

}).WithName("GeefAlleTaken");

// Endpoint voor het ophalen van een specifieke taak.
app.MapGet("/taken/{code}", (TakenBeheer takenBeheer, Guid code) =>
{
    var taak = takenBeheer.GeefTaak(code);

    return taak is not null ? Results.Ok(taak) : Results.NotFound();

}).WithName("GeefTaak");

// Endpoint voor het toevoegen van een nieuwe taak.
app.MapPost("/taken", (TakenBeheer takenBeheer, Taak taak) =>
{
    var nieuweTaak = takenBeheer.VoegTaakToe(taak);

    return Results.Created($"/taken/{nieuweTaak.Code}", nieuweTaak);

}).WithName("VoegTaakToe");

// Endpoint voor het wijzigen van een bestaande taak.
app.MapPut("/taken/{code}", (TakenBeheer takenBeheer, Guid code, Taak taak) =>
{
    taak = taak with { Code = code };

    var gewijzigdeTaak = takenBeheer.WijzigTaak(taak);

    return gewijzigdeTaak is not null ? Results.Ok(gewijzigdeTaak) : Results.NotFound();

}).WithName("WijzigTaak");

// Endpoint voor het voltooien van een taak.
app.MapPut("/taken/{code}/voltooi", (TakenBeheer takenBeheer, Guid code) =>
{
    var voltooideTaak = takenBeheer.VoltooiTaak(code);

    return voltooideTaak is not null ? Results.Ok(voltooideTaak) : Results.NotFound();

}).WithName("VoltooiTaak");

// Endpoint voor het verwijderen van een taak.
app.MapDelete("/taken/{code}", (TakenBeheer takenBeheer, Guid code) =>
{
    var taak = takenBeheer.VerwijderTaak(code);

    return taak is not null ? Results.Ok(taak) : Results.NotFound();

}).WithName("VerwijderTaak");

app.Run();

// Declareer een record waarin de gegevens van een taak kunnen worden opgeslagen.
record Taak(Guid Code, string Titel, bool IsVoltooid);


// Declareer een klasse die verantwoordelijk is voor het beheer van taken.
class TakenBeheer
{
    private readonly List<Taak> _taken = new();

    public TakenBeheer()
    {
        // Voeg een standaard taak toe aan de lijst van taken.
        _taken.Add(new Taak(Guid.NewGuid(), "Starten met het gebruiken van taken", false));
    }

    public List<Taak> GeefAlleTaken()
    {
        // Geef een kopie van de lijst van taken terug.
        return _taken;
    }

    public Taak GeefTaak(Guid code)
    {
        // Zoek naar de taak met de gegeven code.
        return _taken.FirstOrDefault(t => t.Code == code);
    }

    public Taak VoegTaakToe(Taak taak)
    {
        // Maak een nieuwe taak aan met een unieke code.
        var nieuweTaak = taak with { Code = Guid.NewGuid() };

        // Als er nog geen taak met dezelfde titel bestaan...
        if (!_taken.Any(t => t.Titel == taak.Titel))
        {
            // Voeg de nieuwe taak toe.
            _taken.Add(nieuweTaak);
        }

        return nieuweTaak;
    }

    public Taak WijzigTaak(Taak taak)
    {
        // Zoek naar de taak met de gegeven code.
        var bestaandeTaak = _taken.FirstOrDefault(t => t.Code == taak.Code);

        // Als de taak bestaat, wijzig deze met de nieuwe gegevens.
        if (bestaandeTaak != null)
        {
            // Verwijder de oude taak en voeg de gewijzigde taak toe.
            _taken.Remove(bestaandeTaak);
            _taken.Add(taak);

            return taak;
        }

        return bestaandeTaak;
    }

    public Taak VoltooiTaak(Guid code)
    {
        // Zoek naar de taak met de gegeven code.
        var bestaandeTaak = _taken.FirstOrDefault(t => t.Code == code);

        // Als de taak bestaat, wijzig de status van de taak naar voltooid.
        if (bestaandeTaak != null)
        {
            var voltooideTaak = bestaandeTaak with { IsVoltooid = true };

            // Verwijder de oude taak en voeg de gewijzigde taak toe.
            _taken.Remove(bestaandeTaak);
            _taken.Add(voltooideTaak);

            return voltooideTaak;
        }

        return bestaandeTaak;
    }

    public Taak VerwijderTaak(Guid code)
    {
        // Zoek naar de taak met de gegeven code.
        var bestaandeTaak = _taken.FirstOrDefault(t => t.Code == code);

        // Als de taak bestaat, verwijder deze uit de lijst van taken.
        if (bestaandeTaak != null)
        {
            _taken.Remove(bestaandeTaak);
        }

        return bestaandeTaak;
    }
}