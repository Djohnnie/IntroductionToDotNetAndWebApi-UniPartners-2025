# IntroductionToDotNetAndWebApi-UniPartners-2025
ADO.NET (SqlClient, SqlConnection, SqlCommand)

## Een console applicatie maken

We keren terug naar console applicaties om ons te focussen op ADO.NET in een eenvoudige omgeving:

```
dotnet new console --name AdoDotNet
```

Start door een referentie naar de NuGet package voor SqlClient toe te voegen:

```xml
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net9.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="Microsoft.Data.SqlClient" Version="5.2.2" />
  </ItemGroup>

</Project>
```

Vervolgens voeg je de referentie naar de correcte namespace toe:

```csharp
using Microsoft.Data.SqlClient;
```

Daarna, configureer je de "connection string" zodat je kan communiceren met je database:

```csharp
var connectionString = "Server=tcp:unipartners.database.windows.net,1433;Initial Catalog=<your name>;Persist Security Info=False;User ID=dotnet;Password=<password>;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
var dbConnection = new SqlConnection(connectionString);
```

Gebruik een DbCommand zodat je queries kan uitvoeren op de database:

```csharp
var dbCommand = new SqlCommand("SELECT * FROM Verjaardagen", dbConnection);
await dbConnection.OpenAsync();
```

Gebruik een DbReader om de resultaten te verwerken:

```csharp
using var dbReader = await dbCommand.ExecuteReaderAsync();

while (await dbReader.ReadAsync())
{
    Console.WriteLine($"{dbReader["Naam"]} is jarig op {dbReader["Geboortedatum"]}");
}
```

Sluit de databaseconnectie zodra je klaar bent met het verwerken van de resultaten:

```csharp
await dbConnection.CloseAsync();
```

Gebruik een andere DbCommand om nieuwe gegevens aan de database toe te voegen:

```csharp
var dbCommand = new SqlCommand("INSERT INTO Verjaardagen (Naam, Geboortedatum) VALUES (@Naam, @Geboortedatum)", dbConnection);
dbCommand.Parameters.AddWithValue("@Naam", naam);
dbCommand.Parameters.AddWithValue("@Geboortedatum", geboortedatum);

await dbConnection.OpenAsync();
await dbCommand.ExecuteNonQueryAsync();
await dbConnection.CloseAsync();
```

## Je code opkuisen

Nu kan je je code opkuisen door war meer structuur aan te brengen:

```csharp
using Microsoft.Data.SqlClient;


var connectionString = "Server=tcp:unipartners.database.windows.net,1433;Initial Catalog=<jouw naam>;Persist Security Info=False;User ID=dotnet;Password=<paswoord>;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
var dbConnection = new SqlConnection(connectionString);

await GeefAlleVerjaardagen(dbConnection);

await VoegEenVerjaardagToe(dbConnection, "Walter Grootaers", new DateTime(1955, 1, 27));

await GeefAlleVerjaardagen(dbConnection);


static async Task GeefAlleVerjaardagen(SqlConnection dbConnection)
{
    var dbCommand = new SqlCommand("SELECT * FROM Verjaardagen", dbConnection);
    await dbConnection.OpenAsync();

    using var dbReader = await dbCommand.ExecuteReaderAsync();
    
    Console.WriteLine("De volgende personen zijn jarig:");

    while (await dbReader.ReadAsync())
    {
        Console.WriteLine($"{dbReader["Naam"]} is jarig op {dbReader["Geboortedatum"]}");
    }

    Console.WriteLine();

    await dbConnection.CloseAsync();
}

static async Task VoegEenVerjaardagToe(SqlConnection dbConnection, string naam, DateTime geboortedatum)
{
    var dbCommand = new SqlCommand("INSERT INTO Verjaardagen (Naam, Geboortedatum) VALUES (@Naam, @Geboortedatum)", dbConnection);
    dbCommand.Parameters.AddWithValue("@Naam", naam);
    dbCommand.Parameters.AddWithValue("@Geboortedatum", geboortedatum);

    await dbConnection.OpenAsync();
    await dbCommand.ExecuteNonQueryAsync();
    await dbConnection.CloseAsync();
}
```

## Models toevoegen aan je database communicatie

Maak een record aan dat de data voor een verjaardag kan bijhouden:

```csharp
record Verjaardag(string Naam, DateTime Geboortedatum);
```

Maak gebruik van het nieuwe record om de data van en naar de database te verpakken:

```csharp
using Microsoft.Data.SqlClient;


var connectionString = "Server=tcp:unipartners.database.windows.net,1433;Initial Catalog=<jouw naam>;Persist Security Info=False;User ID=dotnet;Password=<paswoord>;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
var dbConnection = new SqlConnection(connectionString);

foreach( var verjaardag in await GeefAlleVerjaardagen(dbConnection))
{
    Console.WriteLine($"{verjaardag.Naam} is jarig op {verjaardag.Geboortedatum:dd-MM-yyyy}");
}

var nieuweVerjaardag = new Verjaardag("Walter Grootaers", new DateTime(1955, 1, 27));
await VoegEenVerjaardagToe(dbConnection, nieuweVerjaardag);

foreach( var verjaardag in await GeefAlleVerjaardagen(dbConnection))
{
    Console.WriteLine($"{verjaardag.Naam} is jarig op {verjaardag.Geboortedatum:dd-MM-yyyy}");
}


static async Task<List<Verjaardag>> GeefAlleVerjaardagen(SqlConnection dbConnection)
{
    var verjaardagen = new List<Verjaardag>();

    var dbCommand = new SqlCommand("SELECT * FROM Verjaardagen", dbConnection);
    await dbConnection.OpenAsync();

    using var dbReader = await dbCommand.ExecuteReaderAsync();

    while (await dbReader.ReadAsync())
    {
        verjaardagen.Add(new Verjaardag((string)dbReader["Naam"], (DateTime)dbReader["Geboortedatum"]));
    }

    await dbConnection.CloseAsync();

    return verjaardagen;
}

static async Task VoegEenVerjaardagToe(SqlConnection dbConnection, Verjaardag verjaardag)
{
    var dbCommand = new SqlCommand("INSERT INTO Verjaardagen (Naam, Geboortedatum) VALUES (@Naam, @Geboortedatum)", dbConnection);
    dbCommand.Parameters.AddWithValue("@Naam", verjaardag.Naam);
    dbCommand.Parameters.AddWithValue("@Geboortedatum", verjaardag.Geboortedatum);

    await dbConnection.OpenAsync();
    await dbCommand.ExecuteNonQueryAsync();
    await dbConnection.CloseAsync();
}
```csharp