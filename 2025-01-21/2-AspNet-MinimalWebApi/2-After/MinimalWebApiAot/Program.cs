using System.Text.Json.Serialization;

var builder = WebApplication.CreateSlimBuilder(args);

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.TypeInfoResolverChain.Insert(0, AppJsonSerializerContext.Default);
});

var app = builder.Build();

app.MapGet("/status", () => Results.Ok());

app.MapPost("/verjaardag", (Persoon persoon) =>
{
    return Results.Ok(persoon with { Leeftijd = persoon.Leeftijd + 1 });
});

app.Run();

record Persoon(string Naam, int Leeftijd);

[JsonSerializable(typeof(Persoon))]
internal partial class AppJsonSerializerContext : JsonSerializerContext
{

}