using TakenLijstControllerWebApi.Beheer;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

// Registreer het TakenBeheer in de Dependency Injection container als singleton.
// Op deze manier zal elke request dezelfde instantie van het TakenBeheer gebruiken
// en dus altijd toegang hebben tot de volledige lijst van taken.
builder.Services.AddSingleton<TakenBeheer>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();