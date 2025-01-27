using Microsoft.Data.SqlClient;


var connectionString = "Server=tcp:unipartners.database.windows.net,1433;Initial Catalog=<jouw naam>;Persist Security Info=False;User ID=dotnet;Password=<paswoord>;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
var dbConnection = new SqlConnection(connectionString);

await GeefAlleVerjaardagen(dbConnection);

await VoegEenVerjaardagToe(dbConnection, "Walter Grootaers", new DateTime(1955, 1, 27));

await GeefAlleVerjaardagen(dbConnection);


static async Task GeefAlleVerjaardagen(SqlConnection dbConnection)
{
    var dbCommand = new SqlCommand("SELECT * FROM Verjaardagen", dbConnection);
    await dbConnection.OpenAsync();

    using var dbReader = await dbCommand.ExecuteReaderAsync();
    
    Console.WriteLine("De volgende personen zijn jarig:");

    while (await dbReader.ReadAsync())
    {
        Console.WriteLine($"{dbReader["Naam"]} is jarig op {dbReader["Geboortedatum"]}");
    }

    Console.WriteLine();

    await dbConnection.CloseAsync();
}

static async Task VoegEenVerjaardagToe(SqlConnection dbConnection, string naam, DateTime geboortedatum)
{
    var dbCommand = new SqlCommand("INSERT INTO Verjaardagen (Naam, Geboortedatum) VALUES (@Naam, @Geboortedatum)", dbConnection);
    dbCommand.Parameters.AddWithValue("@Naam", naam);
    dbCommand.Parameters.AddWithValue("@Geboortedatum", geboortedatum);

    await dbConnection.OpenAsync();
    await dbCommand.ExecuteNonQueryAsync();
    await dbConnection.CloseAsync();
}