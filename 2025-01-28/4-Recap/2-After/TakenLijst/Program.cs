using Microsoft.EntityFrameworkCore;
using Takenlijst;
using Takenlijst.Beheer;
using Takenlijst.DataAccess;
using Takenlijst.Models;
using Takenlijst.Repositories;

var builder = WebApplication.CreateBuilder(args);

var connectionString = "Server=tcp:unipartners.database.windows.net,1433;Initial Catalog=johnny;Persist Security Info=False;User ID=dotnet;Password=BijOnsZitJeGoed!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

builder.Services.AddOpenApi();
builder.Services.AddScoped<TakenBeheer>();
builder.Services.AddScoped<IRepository<Taak>, TakenRepository>();
builder.Services.AddDbContext<TakenDbContext>(options =>
{
    options.UseSqlServer(connectionString);
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.TakenEndpointsToevoegen();

app.Run();