using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddTransient<PersoonGenerator>();
builder.Services.AddTransient<NaamGenerator>();
builder.Services.AddTransient<NummerGenerator>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    // ./openapi/v1.json
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapGet("/status", () =>
{
    return Results.Ok();
}).WithName("GeefStatus");

app.MapGet("/som", (int getal1, int getal2) =>
{
    return Results.Ok(getal1 + getal2);
}).WithName("BerekenSom");

app.MapGet("/persoon", (PersoonGenerator persoonGenerator) =>
{
    return Results.Ok(persoonGenerator.MaakPersoon());
}).WithName("GenereerPersoon");

app.MapPost("/verjaardag", (Persoon persoon) =>
{
    return Results.Ok(persoon with { Leeftijd = persoon.Leeftijd + 1 });
}).WithName("VierVerjaardag");

app.Run();

record Persoon(string Naam, int Leeftijd);

class PersoonGenerator
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

class NaamGenerator
{
    public string GenereerNaam()
    {
        return "Karel";
    }
}

class NummerGenerator
{
    public int GenereerNummer()
    {
        return 42;
    }
}