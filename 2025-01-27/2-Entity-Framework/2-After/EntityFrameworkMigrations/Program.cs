using Microsoft.EntityFrameworkCore;


var db = new MijnDbContext();
db.Database.EnsureCreated();

db.Verjaardagen.Add(new Verjaardag { Naam = "Johnny Hooyberghs", Geboortedatum = new DateTime(1985, 5, 25) });
await db.SaveChangesAsync();



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