# IntroductionToDotNetAndWebApi-UniPartners-2025
Entity Framework (ORM, DbContext, Entity mapping)

## Een nieuwe console applicatie maken

Maak opnieuw een verse console-applicatie:

```
dotnet new console --name EntityFramework
```

Voeg een referentie toe naar de EntityFramework NuGet package van Microsoft:

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

Voeg een referentie toe naar de juiste namespace in je code:

```csharp
using Microsoft.EntityFrameworkCore;
```

Omdat EntityFramework een framework gebaseerd is op Entities en dus modellen, dien je een klasse aan te maken die een verjaardag kan voorstellen:

```csharp
class Verjaardag
{
    public int Code { get; set; }
    public string Naam { get; set; }
    public DateTime Geboortedatum { get; set; }
}
```

Op basis van deze klasse, kan je dan een EntityFramework configuratie toevoegen die de databasestructuur zal mappen:

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

Dankzij de DbSet kunnen we een tabel op een entiteit mappen.
Dankzij de OnConfiguring methode kunnen we de connectie met de database configureren.
Dankzij de OnModelCreating methode kunnen we de entities naar table mapping verder configureren en definiëren dat de Code eigenschap op onze klasse de primary key zal zijn voor deze tabel.

Verder kunnnen we de entiteit(en) en de database context dan verder gebruiken om gegevens uit onze databse te verwerken:

```csharp
using var db = new MijnDbContext();

Console.WriteLine("Verjaardagen:");

foreach (var verjaardag in db.Verjaardagen)
{
    Console.WriteLine($"{verjaardag.Naam} is jarig op {verjaardag.Geboortedatum:dd-MM-yyyy}");
}

db.Verjaardagen.Add(new Verjaardag { Naam = "Walter Grootaers", Geboortedatum = new DateTime(1955, 1, 27) });
await db.SaveChangesAsync();

Console.WriteLine("Verjaardagen:");

foreach (var verjaardag in db.Verjaardagen)
{
    Console.WriteLine($"{verjaardag.Naam} is jarig op {verjaardag.Geboortedatum:dd-MM-yyyy}");
}

Console.ReadKey();
```

## Dependency injection integreren met Entity Framework

Maak opnieuw een verse console-applicatie:

```
dotnet new console --name EntityFrameworkMetDependencyInjection
```

Voeg een referentie toe naar de EntityFramework NuGet package van Microsoft en de package die Dependency Injection voor ons verzorgt:

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
    <PackageReference Include="Microsoft.Extensions.DependencyInjection" Version="9.0.1" />
  </ItemGroup>

</Project>
```

Voeg een referentie toe naar de juiste namespaces in je code:

```csharp
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
```

Maak opnieuw de Verjaardag klasse en de database Context klasse:

```csharp
class Verjaardag
{
    public int Code { get; set; }
    public string Naam { get; set; }
    public DateTime Geboortedatum { get; set; }
}
```

Deze keer hoef je de connection string niet in je database context klasse te configureren:

```csharp
class MijnDbContext : DbContext
{
    public DbSet<Verjaardag> Verjaardagen { get; set; }

    public MijnDbContext(DbContextOptions<MijnDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Verjaardag>().HasKey(x => x.Code);
    }
}
```

Maak nu een extra klasse aan die je gebruikt om database-acties uit te voeren:

```csharp
class VerjaardagHelper
{
    private readonly MijnDbContext _db;

    public VerjaardagHelper(MijnDbContext db)
    {
        _db = db;
    }

    public async Task<List<Verjaardag>> HaalVerjaardagenOp()
    {
        return await _db.Verjaardagen.ToListAsync();
    }

    public async Task VoegVerjaardagToe(string naam, DateTime geboortedatum)
    {
        _db.Verjaardagen.Add(new Verjaardag { Naam = naam, Geboortedatum = geboortedatum });
        await _db.SaveChangesAsync();
    }
}
```

Denkzij Dependency Inject kunnen we nu de database context injecteren in deze extra hulpklasse.

Configureer Dependency Injection in het begin van je code:

```csharp
var services = new ServiceCollection();
services.AddDbContext<MijnDbContext>(o =>
{
    o.UseSqlServer("Server=tcp:unipartners.database.windows.net,1433;Initial Catalog=<jouw naam>;Persist Security Info=False;User ID=dotnet;Password=<paswoord>;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
});
services.AddScoped<VerjaardagHelper>();
```

Bouw de ServiceProvider en haal een referentie op naar de 'VerjaardagHelper':

```csharp
var provider = services.BuildServiceProvider();

var helper = provider.GetRequiredService<VerjaardagHelper>();
```

Gebruik de 'VerjaardagHelper' om gelijkaardige acties uit te voeren zoals voorheen:

```csharp
Console.WriteLine("Verjaardagen:");

foreach (var verjaardag in await helper.HaalVerjaardagenOp())
{
    Console.WriteLine($"{verjaardag.Naam} is jarig op {verjaardag.Geboortedatum:dd-MM-yyyy}");
}

await helper.VoegVerjaardagToe("Walter Grootaers", new DateTime(1955, 1, 27));

Console.WriteLine("Verjaardagen:");

foreach (var verjaardag in await helper.HaalVerjaardagenOp())
{
    Console.WriteLine($"{verjaardag.Naam} is jarig op {verjaardag.Geboortedatum:dd-MM-yyyy}");
}

Console.ReadKey();
```

## Entity Framework Migrations

Dankzij Entity Framework Migrations kan je eenvoudig je database up-to-date houden en automatisch de databasestructuur bijwerken:

Maak opnieuw een console applicatie met EntityFramework als package reference:

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

Voeg een gelijkaardig stukje code toe om een entiteit (of meerdere entiteiten) te maken en een database context om de configuratie te doen:

```csharp
class Verjaardag
{
    public int Code { get; set; }
    public string Naam { get; set; }
    public DateTime Geboortedatum { get; set; }
}
```

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

Voeg nu de volgende code toe om een databse-update uit te voeren:

```csharp
var db = new MijnDbContext();
db.Database.EnsureCreated();

db.Verjaardagen.Add(new Verjaardag { Naam = "Johnny Hooyberghs", Geboortedatum = new DateTime(1985, 5, 25) });
await db.SaveChangesAsync();
```