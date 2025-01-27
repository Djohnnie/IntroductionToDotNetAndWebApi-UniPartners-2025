# IntroductionToDotNetAndWebApi-UniPartners-2025
LINQ (LINQ, Fluent API's, Lambda Expressions, Expression Trees)

## Een nieuwe console applicatie maken

Maak opnieuw een nieuwe console-applicatie:

```
dotnet new console --name Linq
```

Maak een klasse waarmee je een verjaardag kan voorstellen:

```csharp
class Verjaardag
{
    public string Naam { get; set; }
    public DateTime Geboortedatum { get; set; }
    public bool IsFamilie { get; set; }
}
```

Bereid een lijstje van verjaardagen voor in het geheugen van je applicatie:

```csharp
var verjaardagen = new List<Verjaardag>
{
    new Verjaardag { Naam = "Piet", Geboortedatum = new DateTime(1990, 1, 1), IsFamilie = true },
    new Verjaardag { Naam = "Klaas", Geboortedatum = new DateTime(1991, 2, 2), IsFamilie = false },
    new Verjaardag { Naam = "Jan", Geboortedatum = new DateTime(1992, 3, 3), IsFamilie = true },
    new Verjaardag { Naam = "Kees", Geboortedatum = new DateTime(1993, 4, 4), IsFamilie = false },
};
```

Maak gebruik van Linq om alle verjaardagen op te zoeken van familie:

```csharp
Console.WriteLine("-----------------------------");

var familieleden = verjaardagen.Where(v => v.IsFamilie).ToList();
familieleden.ForEach(f => Console.WriteLine($"{f.Naam} is familie."));
```

Er is ook Linq ForEach gebruikt om een Console.WriteLine uit te voeren voor elk resultaat.

Gebruik vervolgens Linq om de gegevens te sorteren op basis van de geboortedatum.

```csharp
Console.WriteLine("-----------------------------");

var gesorteerd = verjaardagen.OrderByDescending(v => v.Geboortedatum).ToList();
gesorteerd.ForEach(g => Console.WriteLine($"{g.Naam} is geboren op {g.Geboortedatum.ToShortDateString()}."));
```

Gebruik ten laatste Linq om een transformatie toe te passen van een verjaardag naar een gesorteerde opsomming van namen:

```csharp
Console.WriteLine("-----------------------------");

var namen = string.Join(", ", verjaardagen.OrderBy(x => x.Naam).Select(v => v.Naam.ToUpper()));
Console.WriteLine($"De namen zijn: {namen}.");
```

## Linq en Entity Framework

Maak opnieuw een nieuwe console-applicatie:

```
dotnet new console --name EntityFrameworkLinq
```

Voeg opnieuw EntityFramework toe als referentie:

```xml
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net9.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="Microsoft.EntityFrameworkCore.SqlServer" Version="9.0.1" />
  </ItemGroup>

</Project>
```

Voeg de referentie naar de juiste namespace toe:

```csharp
using Microsoft.EntityFrameworkCore;
```

Maak een klasse waarmee je een verjaardag kan voorstellen:

```csharp
class Verjaardag
{
    public string Naam { get; set; }
    public DateTime Geboortedatum { get; set; }
    public bool IsFamilie { get; set; }
}
```

Configureer Entity Framework om met een database te communiceren, op basis van deze entiteit:

```csharp
class MijnDbContext : DbContext
{
    public DbSet<Verjaardag> Verjaardagen { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var connectionString = "Server=tcp:unipartners.database.windows.net,1433;Initial Catalog=<jouw naam>;Persist Security Info=False;User ID=dotnet;Password=<paswoord>;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        optionsBuilder.UseSqlServer(connectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Verjaardag>().HasKey(x => x.Code);
    }
}
```

Voeg de volgende code toe om Entity Framework en Linq met elkaar te combineren:

```csharp
Console.WriteLine("Verjaardagen:");

var verjaardagen = await db.Verjaardagen.OrderBy(x => x.Geboortedatum).ToListAsync();

foreach (var verjaardag in verjaardagen)
{
    Console.WriteLine($"{verjaardag.Naam} is jarig op {verjaardag.Geboortedatum:dd-MM-yyyy}");
}
```

Voeg een nieuwe verjaardag toe:

```csharp
db.Verjaardagen.Add(new Verjaardag { Naam = "Walter Grootaers", Geboortedatum = new DateTime(1955, 1, 24) });
await db.SaveChangesAsync();
```

Zoek via Linq alle verjaardagen terug die op vandaag vallen:

```csharp
Console.WriteLine("Verjaardagen vandaag:");

verjaardagen = await db.Verjaardagen.Where(x => x.Geboortedatum.Month == DateTime.Today.Month && x.Geboortedatum.Day == DateTime.Today.Day).ToListAsync();

foreach (var verjaardag in verjaardagen)
{
    Console.WriteLine($"{verjaardag.Naam} is jarig op {verjaardag.Geboortedatum:dd-MM-yyyy}");
}
```