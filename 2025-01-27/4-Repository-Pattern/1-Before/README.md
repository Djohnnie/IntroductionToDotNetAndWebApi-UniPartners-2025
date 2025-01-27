# IntroductionToDotNetAndWebApi-UniPartners-2025
Repository pattern (Abstraction, DI, CRUD)

## Een console app aanmaken

Maak opnieuw een nieuwe console-applicatie:

```
dotnet new console --name RepositoryPatternAdoDotNet
```

Voeg ook opnieuw de SqlClient NuGet package reference en namespace toe aan je project:

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

```csharp
using System.Data.Common;
using Microsoft.Data.SqlClient;
```

Voeg het record toe dat de gegevens voor verjaardagen kan opslaan:

```csharp
record Verjaardag(string Naam, DateTime Geboortedatum);
```

Voeg een abstracte klasse toe, die als basis kan dienen voor al onze toekomstige repositories:

```csharp
abstract class Repository<T>
{
    protected readonly SqlConnection _dbConnection;
    protected readonly string _tableName;

    public Repository(SqlConnection dbConnection, string tableName)
    {
        _dbConnection = dbConnection;
        _tableName = tableName;
    }

    protected abstract T RecordOmzetten(DbDataReader dbReader);
    protected abstract string[] QueryParameters();
    protected abstract SqlParameter[] ParametersOmzetten(T item);

    public async Task<List<T>> HaalOp()
    {
        var result = new List<T>();

        var dbCommand = new SqlCommand($"SELECT * FROM {_tableName}", _dbConnection);
        await _dbConnection.OpenAsync();
        using var dbReader = await dbCommand.ExecuteReaderAsync();
        while (await dbReader.ReadAsync())
        {
            result.Add(RecordOmzetten(dbReader));
        }
        await _dbConnection.CloseAsync();

        return result;
    }

    public async Task VoegToe(T item)
    {
        var kolommon = string.Join(", ", QueryParameters());
        var parameters = string.Join(", ", QueryParameters().Select(p => $"@{p}"));
        var dbCommand = new SqlCommand($"INSERT INTO {_tableName} ({kolommon}) VALUES ({parameters})", _dbConnection);
        dbCommand.Parameters.AddRange(ParametersOmzetten(item));

        await _dbConnection.OpenAsync();
        await dbCommand.ExecuteNonQueryAsync();
        await _dbConnection.CloseAsync();
    }
}
```

Voeg een implementatie toe die specifiek voor Verjaardagen kan dienen:

```csharp
class VerjaardagRepository : Repository<Verjaardag>
{
    public VerjaardagRepository(SqlConnection dbConnection) : base(dbConnection, "Verjaardagen")
    {
    }

    protected override Verjaardag RecordOmzetten(DbDataReader dbReader)
    {
        return new Verjaardag((string)dbReader["Naam"], (DateTime)dbReader["Geboortedatum"]);
    }

    protected override string[] QueryParameters()
    {
        return [ "Naam", "Geboortedatum" ];
    }

    protected override SqlParameter[] ParametersOmzetten(Verjaardag item)
    {
        return
        [
            new SqlParameter("@Naam", item.Naam ),
            new SqlParameter("@Geboortedatum", item.Geboortedatum )
        ];
    }
}
```

Gebruik de Repository klasse om te communiceren met de database:

```csharp
var connectionString = "Server=tcp:unipartners.database.windows.net,1433;Initial Catalog=<jouw naam>;Persist Security Info=False;User ID=dotnet;Password=<paswoord>;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
var dbConnection = new SqlConnection(connectionString);
var verjaardagRepository = new VerjaardagRepository(dbConnection);

foreach (var verjaardag in await verjaardagRepository.HaalOp())
{
    Console.WriteLine($"{verjaardag.Naam} is jarig op {verjaardag.Geboortedatum:dd-MM-yyyy}");
}

var nieuweVerjaardag = new Verjaardag("Walter Grootaers", new DateTime(1955, 1, 27));
await verjaardagRepository.VoegToe(nieuweVerjaardag);

foreach (var verjaardag in await verjaardagRepository.HaalOp())
{
    Console.WriteLine($"{verjaardag.Naam} is jarig op {verjaardag.Geboortedatum:dd-MM-yyyy}");
}

Console.ReadKey();
```

## Een Repository gebruiken met Entity Framework

Maak opnieuw een nieuwe console-applicatie:

```
dotnet new console --name RepositoryPatternEntityFramework
```

Voeg ook opnieuw de Entity Framework NuGet package en Dependency Injection package reference en namespaces toe aan je project:

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

```csharp
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
```

Maak een nieuwe klasse aan om Verjaardagen voor te stellen:

```csharp
class Verjaardag
{
    public int Code { get; set; }
    public string Naam { get; set; }
    public DateTime Geboortedatum { get; set; }
}
```

Configureer een Entity Framework database context:

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

Start met een generieke Repository interface die het contract zal definiëren:

```csharp
interface IRepository<T>
{
    Task<List<T>> HaalOp();
    Task VoegToe(T item);
}
```

Implementeer dan een generieke Repository, op basis van die interface, die we voor alle toekomstige Repositories kunnen gebruiken:

```csharp
class Repository<T> : IRepository<T> where T : class
{
    private readonly MijnDbContext _db;
    private readonly DbSet<T> _set;

    public Repository(MijnDbContext db, DbSet<T> set)
    {
        _db = db;
        _set = set;
    }

    public async Task<List<T>> HaalOp()
    {
        return await _db.Set<T>().ToListAsync();
    }

    public async Task VoegToe(T item)
    {
        _db.Set<T>().Add(item);
        await _db.SaveChangesAsync();
    }
}
```

Maak nu ook een implementatie, specifiek voor verjaardagen.

```csharp
class VerjaardagRepository : Repository<Verjaardag>
{
    private readonly MijnDbContext _db;

    public VerjaardagRepository(MijnDbContext db) : base(db, db.Verjaardagen) { }
}
```

En gebruik uiteindelijk deze implementatie, met z'n abstracte versie, om te registreren via Dependency Injection:

```csharp
var services = new ServiceCollection();
services.AddDbContext<MijnDbContext>(o =>
{
    o.UseSqlServer("Server=tcp:unipartners.database.windows.net,1433;Initial Catalog=<jouw naam>;Persist Security Info=False;User ID=dotnet;Password=<paswoord>;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
});
services.AddScoped<IRepository<Verjaardag>, VerjaardagRepository>();
```

Gebruik nu de ServiceProvider om een instantie te creëren:

```csharp
var provider = services.BuildServiceProvider();

var repository = provider.GetRequiredService<IRepository<Verjaardag>>();
```

En ten slotte kan je de repository gebruiken om effectief met de database te communiceren:

```csharp
foreach (var verjaardag in await repository.HaalOp())
{
    Console.WriteLine($"{verjaardag.Naam} is jarig op {verjaardag.Geboortedatum:dd-MM-yyyy}");
}

await repository.VoegToe(new Verjaardag { Naam = "Walter Grootaers", Geboortedatum = new DateTime(1955, 1, 27) });

Console.WriteLine("Verjaardagen:");

foreach (var verjaardag in await repository.HaalOp())
{
    Console.WriteLine($"{verjaardag.Naam} is jarig op {verjaardag.Geboortedatum:dd-MM-yyyy}");
}

Console.ReadKey();
```