using System.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddAuthentication();
builder.Services.AddAuthorization();
builder.Services.AddDbContext<MijnDbContext>(options =>
    options.UseInMemoryDatabase("MijnDatabase"));
builder.Services.AddIdentityApiEndpoints<Gebruiker>()
    .AddEntityFrameworkStores<MijnDbContext>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapGet("/status", (HttpContext http) =>
{
    var message = $"Hello, {http.User.Identity.Name}";
    return Results.Ok(message);
}).WithName("GeefStatus");

app.MapGroup("/auth").MapIdentityApi<Gebruiker>();

app.Run();

public class Gebruiker : IdentityUser
{

}

public class MijnDbContext : IdentityDbContext<Gebruiker>
{
    public MijnDbContext(DbContextOptions<MijnDbContext> options)
      : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.LogTo(message => Debug.WriteLine(message));

        base.OnConfiguring(optionsBuilder);
    }
}