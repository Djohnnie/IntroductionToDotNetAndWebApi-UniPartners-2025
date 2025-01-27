using Microsoft.EntityFrameworkCore;


using var db = new MijnDbContext();

Console.WriteLine("Verjaardagen:");

var verjaardagen = await db.Verjaardagen.OrderBy(x => x.Geboortedatum).ToListAsync();

foreach (var verjaardag in verjaardagen)
{
    Console.WriteLine($"{verjaardag.Naam} is jarig op {verjaardag.Geboortedatum:dd-MM-yyyy}");
}

db.Verjaardagen.Add(new Verjaardag { Naam = "Walter Grootaers", Geboortedatum = new DateTime(1955, 1, 24) });
await db.SaveChangesAsync();

Console.WriteLine("Verjaardagen vandaag:");

verjaardagen = await db.Verjaardagen.Where(x => x.Geboortedatum.Month == DateTime.Today.Month && x.Geboortedatum.Day == DateTime.Today.Day).ToListAsync();

foreach (var verjaardag in verjaardagen)
{
    Console.WriteLine($"{verjaardag.Naam} is jarig op {verjaardag.Geboortedatum:dd-MM-yyyy}");
}

Console.ReadKey();


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

class Verjaardag
{
    public int Code { get; set; }
    public string Naam { get; set; }
    public DateTime Geboortedatum { get; set; }
}