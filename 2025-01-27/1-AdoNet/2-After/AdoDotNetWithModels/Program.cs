using Microsoft.Data.SqlClient;


var connectionString = "Server=tcp:unipartners.database.windows.net,1433;Initial Catalog=<jouw naam>;Persist Security Info=False;User ID=dotnet;Password=<paswoord>;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
var dbConnection = new SqlConnection(connectionString);

foreach( var verjaardag in await GeefAlleVerjaardagen(dbConnection))
{
    Console.WriteLine($"{verjaardag.Naam} is jarig op {verjaardag.Geboortedatum:dd-MM-yyyy}");
}

var nieuweVerjaardag = new Verjaardag("Walter Grootaers", new DateTime(1955, 1, 27));
await VoegEenVerjaardagToe(dbConnection, nieuweVerjaardag);

foreach( var verjaardag in await GeefAlleVerjaardagen(dbConnection))
{
    Console.WriteLine($"{verjaardag.Naam} is jarig op {verjaardag.Geboortedatum:dd-MM-yyyy}");
}


static async Task<List<Verjaardag>> GeefAlleVerjaardagen(SqlConnection dbConnection)
{
    var verjaardagen = new List<Verjaardag>();

    var dbCommand = new SqlCommand("SELECT * FROM Verjaardagen", dbConnection);
    await dbConnection.OpenAsync();

    using var dbReader = await dbCommand.ExecuteReaderAsync();

    while (await dbReader.ReadAsync())
    {
        verjaardagen.Add(new Verjaardag((string)dbReader["Naam"], (DateTime)dbReader["Geboortedatum"]));
    }

    await dbConnection.CloseAsync();

    return verjaardagen;
}

static async Task VoegEenVerjaardagToe(SqlConnection dbConnection, Verjaardag verjaardag)
{
    var dbCommand = new SqlCommand("INSERT INTO Verjaardagen (Naam, Geboortedatum) VALUES (@Naam, @Geboortedatum)", dbConnection);
    dbCommand.Parameters.AddWithValue("@Naam", verjaardag.Naam);
    dbCommand.Parameters.AddWithValue("@Geboortedatum", verjaardag.Geboortedatum);

    await dbConnection.OpenAsync();
    await dbCommand.ExecuteNonQueryAsync();
    await dbConnection.CloseAsync();
}

record Verjaardag(string Naam, DateTime Geboortedatum);