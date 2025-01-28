using Takenlijst.Beheer;
using Takenlijst.Dto;
using Takenlijst.Models;

namespace Takenlijst;

public static class EndpointExtensions
{
    public static void TakenEndpointsToevoegen(this IEndpointRouteBuilder endpoints)
    {
        // Endpoint voor het ophalen van alle taken.
        endpoints.MapGet("/taken", async (TakenBeheer takenBeheer) =>
        {
            var taken = await takenBeheer.GeefAlleTaken();

            return Results.Ok(taken);

        }).WithName("GeefAlleTaken");

        // Endpoint voor het ophalen van een specifieke taak.
        endpoints.MapGet("/taken/{code}", async (TakenBeheer takenBeheer, Guid code) =>
        {
            var taak = await takenBeheer.GeefTaak(code);

            return taak is not null ? Results.Ok(taak) : Results.NotFound();

        }).WithName("GeefTaak");

        // Endpoint voor het toevoegen van een nieuwe taak.
        endpoints.MapPost("/taken", async (TakenBeheer takenBeheer, ToeTeVoegenTaakDto taak) =>
        {
            var nieuweTaak = await takenBeheer.VoegTaakToe(taak);

            return Results.Created($"/taken/{nieuweTaak.Code}", nieuweTaak);

        }).WithName("VoegTaakToe");

        // Endpoint voor het wijzigen van een bestaande taak.
        endpoints.MapPut("/taken/{code}", async (TakenBeheer takenBeheer, Guid code, TeWijzigenTaakDto taak) =>
        {
            var gewijzigdeTaak = await takenBeheer.WijzigTaak(code, taak);

            return gewijzigdeTaak is not null ? Results.Ok(gewijzigdeTaak) : Results.NotFound();

        }).WithName("WijzigTaak");

        // Endpoint voor het voltooien van een taak.
        endpoints.MapPut("/taken/{code}/voltooi", async (TakenBeheer takenBeheer, Guid code) =>
        {
            var voltooideTaak = await takenBeheer.VoltooiTaak(code);

            return voltooideTaak is not null ? Results.Ok(voltooideTaak) : Results.NotFound();

        }).WithName("VoltooiTaak");

        // Endpoint voor het verwijderen van een taak.
        endpoints.MapDelete("/taken/{code}", async (TakenBeheer takenBeheer, Guid code) =>
        {
            var taak = await takenBeheer.VerwijderTaak(code);

            return taak is not null ? Results.Ok(taak) : Results.NotFound();

        }).WithName("VerwijderTaak");
    }
}