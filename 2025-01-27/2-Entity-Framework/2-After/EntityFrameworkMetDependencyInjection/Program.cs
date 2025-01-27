using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();
services.AddDbContext<MijnDbContext>(o =>
{
    o.UseSqlServer("Server=tcp:unipartners.database.windows.net,1433;Initial Catalog=<jouw naam>;Persist Security Info=False;User ID=dotnet;Password=<paswoord>;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
});
services.AddScoped<VerjaardagHelper>();

var provider = services.BuildServiceProvider();

var helper = provider.GetRequiredService<VerjaardagHelper>();

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

class Verjaardag
{
    public int Code { get; set; }
    public string Naam { get; set; }
    public DateTime Geboortedatum { get; set; }
}