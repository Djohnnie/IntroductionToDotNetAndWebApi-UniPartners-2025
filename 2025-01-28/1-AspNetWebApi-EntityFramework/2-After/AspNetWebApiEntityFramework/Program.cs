using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
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

app.MapGet("/verjaardagen", async (MijnDbContext dbContext) =>
{
    var verjaardagen = await dbContext.Verjaardagen.ToListAsync();

    return Results.Ok(verjaardagen);
}).WithName("GeefVerjaardagen");

app.MapGet("/verjaardagen/{code}", async (MijnDbContext dbContext, int code) =>
{
    var verjaardag = await dbContext.Verjaardagen.SingleOrDefaultAsync(x => x.Code == code);

    return verjaardag is not null ? Results.Ok(verjaardag) : Results.NotFound();
}).WithName("GeefVerjaardag");

app.MapPost("/verjaardagen", async (MijnDbContext dbContext, Verjaardag verjaardag) =>
{
    dbContext.Verjaardagen.Add(verjaardag);
    await dbContext.SaveChangesAsync();

    return Results.Created($"/verjaardagen/{verjaardag.Code}", verjaardag);
}).WithName("VoegVerjaardagToe");

app.Run();


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