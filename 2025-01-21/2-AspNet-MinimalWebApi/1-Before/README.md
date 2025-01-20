# IntroductionToDotNetAndWebApi-UniPartners-2025
ASP.NET Minimal Api's (Introductie, DI)

## Een minimal API service maken

Maak een nieuw ASP.NET WebApi project aan:

```
dotnet new webapi --name MinimalWebApi
```

Maak de gegenereerde template-code wat schoon totdat je dit overhoudt:

```csharp
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

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

app.Run();
```

Maak een nieuw endpoint bij dat er als volgt uitziet:

```csharp
app.MapGet("/som", (int getal1, int getal2) =>
{
    return Results.Ok(getal1 + getal2);
}).WithName("BerekenSom");
```

Installeer de REST Client extensie in Visual Studio Code!

Voeg een 'rest.http' bestand toe aan het project en voeg het volgende toe:

```py
@baseUrl = http://localhost:????

### GeefStatus
GET {{baseUrl}}/status

### BerekenSom
@getal1 = 5
@getal2 = 3
GET {{baseUrl}}/som?getal1={{getal1}}&getal2={{getal2}}
```

Voeg het volgende record toe aan de code van je WebApi:

```csharp
record Persoon(string Naam, int Leeftijd);
```

Voeg het volgende endpoint toe aan de code van je WebApi:

```csharp
app.MapPost("/verjaardag", (Persoon persoon) =>
{
    return Results.Ok(persoon with { Leeftijd = persoon.Leeftijd + 1 });
}).WithName("VierVerjaardag");
```

Voeg de volgende test toe aan je 'rest.http' bestand:

```py
### VierVerjaardag
POST {{baseUrl}}/verjaardag
Content-Type: application/json

{
    "naam": "Jozef",
    "leeftijd": 12
}
```

## Dependency Injection in een Minimal WebApi

Voeg de volgende klassen toe aan je WebApi project:

```csharp
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
```

Voeg de volgende configuratie toe aan je dependency injection container:

```csharp
builder.Services.AddTransient<PersoonGenerator>();
builder.Services.AddTransient<NaamGenerator>();
builder.Services.AddTransient<NummerGenerator>();
```

Voeg het volgende endpoint toe aan de code van je WebApi:

```csharp
app.MapGet("/persoon", (PersoonGenerator persoonGenerator) =>
{
    return Results.Ok(persoonGenerator.MaakPersoon());
}).WithName("GenereerPersoon");
```

Voeg de volgende test toe aan je 'rest.http' bestand:

```py
### GenereerPersoon
GET {{baseUrl}}/persoon
```

## .NET Minimal WebApi & AOT (Ahead Of Time)

Maak een nieuw ASP.NET WebApi met AOT project aan:

```
dotnet new webapiaot --name MinimalWebApiAot
```

Maak de gegenereerde template-code wat schoon totdat je dit overhoudt:

```csharp
using System.Text.Json.Serialization;

var builder = WebApplication.CreateSlimBuilder(args);

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.TypeInfoResolverChain.Insert(0, AppJsonSerializerContext.Default);
});

var app = builder.Build();

app.MapGet("/status", () => Results.Ok());

app.Run();

record Persoon(string Naam, int Leeftijd);

[JsonSerializable(typeof(Persoon))]
internal partial class AppJsonSerializerContext : JsonSerializerContext
{

}
```

Voeg het endpoint toe dat gebruik maakte van het record 'Persoon':

```csharp
app.MapPost("/verjaardag", (Persoon persoon) =>
{
    return Results.Ok(persoon with { Leeftijd = persoon.Leeftijd + 1 });
});
```

Voeg opnieuw een 'rest.http' bestand toe waarin de endpoints kunnen worden uitgetest:

```py
@baseUrl = http://localhost:????

### GeefStatus
GET {{baseUrl}}/status

### VierVerjaardag
POST {{baseUrl}}/verjaardag
Content-Type: application/json

{
    "naam": "Steven",
    "leeftijd": 23
}
```

Gebruik IlSpy om na te kijken hoe de code werd aangevuld door de compiler:

```csharp
[CompilerGenerated]
internal class Program
{
	private static void <Main>$(string[] args)
	{
		WebApplicationBuilder builder = WebApplication.CreateSlimBuilder(args);
		builder.Services.ConfigureHttpJsonOptions(delegate(JsonOptions options)
		{
			options.SerializerOptions.TypeInfoResolverChain.Insert(0, AppJsonSerializerContext.Default);
		});
		WebApplication app = builder.Build();
		app.MapGet0("/status", (Func<IResult>)(() => Results.Ok()));
		app.MapPost1("/verjaardag", (Func<Persoon, IResult>)((Persoon persoon) => Results.Ok(persoon with
		{
			Leeftijd = persoon.Leeftijd + 1
		})));
		app.Run();
	}
}
```

Let vooral op de Extension Methods 'MapGet0' en 'MapPost1':

```csharp
[<GeneratedRouteBuilderExtensions_g>FC9003A3F094361C2AC592879CD1B222273B8094C47A0322BED2CADAEF34C5A36__InterceptsLocation(1, "xcgE8GdgBU56sU8ZK4J1XyYBAABQcm9ncmFtLmNz")]
	internal static RouteHandlerBuilder MapGet0(this IEndpointRouteBuilder endpoints, [StringSyntax("Route")] string pattern, Delegate handler)
    {
        // ...
    }

[<GeneratedRouteBuilderExtensions_g>FC9003A3F094361C2AC592879CD1B222273B8094C47A0322BED2CADAEF34C5A36__InterceptsLocation(1, "xcgE8GdgBU56sU8ZK4J1X1QBAABQcm9ncmFtLmNz")]
	internal static RouteHandlerBuilder MapPost1(this IEndpointRouteBuilder endpoints, [StringSyntax("Route")] string pattern, Delegate handler)
    {
        // ...
    }
```