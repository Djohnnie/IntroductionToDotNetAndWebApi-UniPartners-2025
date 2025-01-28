using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddScoped<VerjaardagenBeheer>();
builder.Services.AddDbContext<MijnDbContext>(o =>
{
    o.UseSqlServer("Server=tcp:unipartners.database.windows.net,1433;Initial Catalog=johnny;Persist Security Info=False;User ID=dotnet;Password=BijOnsZitJeGoed!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapGet("/verjaardagen", async (VerjaardagenBeheer beheer) =>
{
    var verjaardagen = await beheer.GeefVerjaardagen();

    return Results.Ok(verjaardagen);
}).WithName("GeefVerjaardagen");

app.MapGet("/verjaardagen/{code}", async (VerjaardagenBeheer beheer, int code) =>
{
    var verjaardag = await beheer.GeefVerjaardag(code);

    return verjaardag is not null ? Results.Ok(verjaardag) : Results.NotFound();
}).WithName("GeefVerjaardag");

app.MapPost("/verjaardagen", async (VerjaardagenBeheer beheer, VerjaardagToeTeVoegenDto verjaardagToeTeVoegen) =>
{
    var verjaardag = await beheer.VoegVerjaardagToe(verjaardagToeTeVoegen);

    return Results.Created($"/verjaardagen/{verjaardag.Code}", verjaardag);
}).WithName("VoegVerjaardagToe");

app.Run();

class VerjaardagenBeheer
{
    private readonly MijnDbContext _dbContext;

    public VerjaardagenBeheer(MijnDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<VerjaardagDto>> GeefVerjaardagen()
    {
        return await _dbContext.Verjaardagen
            .Select(x => new VerjaardagDto(x.Code, x.Naam, x.Geboortedatum))
            .ToListAsync();
    }

    public async Task<VerjaardagDto> GeefVerjaardag(int code)
    {
        return await _dbContext.Verjaardagen
            .Select(x => new VerjaardagDto(x.Code, x.Naam, x.Geboortedatum))
            .SingleOrDefaultAsync(x => x.Code == code);
    }

    public async Task<VerjaardagDto> VoegVerjaardagToe(VerjaardagToeTeVoegenDto verjaardagToeTeVoegen)
    {
        var verjaardag = new Verjaardag
        {
            Naam = verjaardagToeTeVoegen.Naam,
            Geboortedatum = verjaardagToeTeVoegen.Geboortedatum
        };

        _dbContext.Verjaardagen.Add(verjaardag);
        await _dbContext.SaveChangesAsync();

        return new VerjaardagDto(verjaardag.Code, verjaardag.Naam, verjaardag.Geboortedatum);
    }
}

record VerjaardagDto(int Code, string Naam, DateTime Geboortedatum)
{
    public string Link => $"/verjaardagen/{Code}";
}

record VerjaardagToeTeVoegenDto(string Naam, DateTime Geboortedatum);

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