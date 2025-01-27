using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();
services.AddDbContext<MijnDbContext>(o =>
{
    o.UseSqlServer("Server=tcp:unipartners.database.windows.net,1433;Initial Catalog=<jouw naam>;Persist Security Info=False;User ID=dotnet;Password=<paswoord>;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
});
services.AddScoped<IRepository<Verjaardag>, VerjaardagRepository>();

var provider = services.BuildServiceProvider();

var repository = provider.GetRequiredService<IRepository<Verjaardag>>();

Console.WriteLine("Verjaardagen:");

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


interface IRepository<T>
{
    Task<List<T>> HaalOp();
    Task VoegToe(T item);
}

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

class VerjaardagRepository : Repository<Verjaardag>
{
    private readonly MijnDbContext _db;

    public VerjaardagRepository(MijnDbContext db) : base(db, db.Verjaardagen) { }
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