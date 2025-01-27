using System.Data.Common;
using Microsoft.Data.SqlClient;


var connectionString = "Server=tcp:unipartners.database.windows.net,1433;Initial Catalog=<jouw naam>;Persist Security Info=False;User ID=dotnet;Password=<paswoord>;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
var dbConnection = new SqlConnection(connectionString);
var verjaardagRepository = new VerjaardagRepository(dbConnection);

foreach (var verjaardag in await verjaardagRepository.HaalOp())
{
    Console.WriteLine($"{verjaardag.Naam} is jarig op {verjaardag.Geboortedatum:dd-MM-yyyy}");
}

var nieuweVerjaardag = new Verjaardag("Walter Grootaers", new DateTime(1955, 1, 27));
await verjaardagRepository.VoegToe(nieuweVerjaardag);

foreach (var verjaardag in await verjaardagRepository.HaalOp())
{
    Console.WriteLine($"{verjaardag.Naam} is jarig op {verjaardag.Geboortedatum:dd-MM-yyyy}");
}

Console.ReadKey();

record Verjaardag(string Naam, DateTime Geboortedatum);

abstract class Repository<T>
{
    protected readonly SqlConnection _dbConnection;
    protected readonly string _tableName;

    public Repository(SqlConnection dbConnection, string tableName)
    {
        _dbConnection = dbConnection;
        _tableName = tableName;
    }

    protected abstract T RecordOmzetten(DbDataReader dbReader);
    protected abstract string[] QueryParameters();
    protected abstract SqlParameter[] ParametersOmzetten(T item);

    public async Task<List<T>> HaalOp()
    {
        var result = new List<T>();

        var dbCommand = new SqlCommand($"SELECT * FROM {_tableName}", _dbConnection);
        await _dbConnection.OpenAsync();
        using var dbReader = await dbCommand.ExecuteReaderAsync();
        while (await dbReader.ReadAsync())
        {
            result.Add(RecordOmzetten(dbReader));
        }
        await _dbConnection.CloseAsync();

        return result;
    }

    public async Task VoegToe(T item)
    {
        var kolommon = string.Join(", ", QueryParameters());
        var parameters = string.Join(", ", QueryParameters().Select(p => $"@{p}"));
        var dbCommand = new SqlCommand($"INSERT INTO {_tableName} ({kolommon}) VALUES ({parameters})", _dbConnection);
        dbCommand.Parameters.AddRange(ParametersOmzetten(item));

        await _dbConnection.OpenAsync();
        await dbCommand.ExecuteNonQueryAsync();
        await _dbConnection.CloseAsync();
    }
}

class VerjaardagRepository : Repository<Verjaardag>
{
    public VerjaardagRepository(SqlConnection dbConnection) : base(dbConnection, "Verjaardagen")
    {
    }

    protected override Verjaardag RecordOmzetten(DbDataReader dbReader)
    {
        return new Verjaardag((string)dbReader["Naam"], (DateTime)dbReader["Geboortedatum"]);
    }

    protected override string[] QueryParameters()
    {
        return [ "Naam", "Geboortedatum" ];
    }

    protected override SqlParameter[] ParametersOmzetten(Verjaardag item)
    {
        return
        [
            new SqlParameter("@Naam", item.Naam ),
            new SqlParameter("@Geboortedatum", item.Geboortedatum )
        ];
    }
}