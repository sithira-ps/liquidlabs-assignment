
using liquidlabs_assignment.Models;

namespace liquidlabs_assignment.Repositories;

public class CountriesRepository : ICountriesRepository
{
    private readonly IDbConnectionFactory _dbCon;
    public CountriesRepository(IDbConnectionFactory dbCon)
    {
        _dbCon = dbCon;
    }
    public async Task<IEnumerable<Country>> GetAllAsync()
    {
        List<Country> countries = [];
        using var conn = _dbCon.CreateConnection();
        await conn.OpenAsync();

        string query = $@"
        SELECT * 
            FROM countries 
        WHERE 
            sync_level = {(int)SyncLevel.all}
        ";

        using var command = conn.CreateCommand();
        command.CommandText = query;
        using var reader = await command.ExecuteReaderAsync();

        while (reader.Read()) // returns true if there's a row to read or false when there are no more rows
        {
            countries.Add(new Country
            {
                Uuid = reader.GetString(reader.GetOrdinal("Id")),
                Name = reader.GetString(reader.GetOrdinal("Name")),
                Continent = reader.GetString(reader.GetOrdinal("Continent")),
                SyncLevel = (SyncLevel)reader.GetInt32(reader.GetOrdinal("sync_level"))
            });
        }

        return countries;
    }

    public async Task<IEnumerable<Country?>> GetByContinentAsync(string continent)
    {
        List<Country> countries = [];
        using var conn = _dbCon.CreateConnection();
        await conn.OpenAsync();

        string query = $@"
        SELECT * 
            FROM countries 
        WHERE 
            LOWER(continent) = @continentName AND 
            sync_level IN ({(int)SyncLevel.continent}, {(int)SyncLevel.all})
        ";

        using var command = conn.CreateCommand();
        command.CommandText = query;

        var para = command.CreateParameter();
        para.ParameterName = "continentName";
        para.Value = continent.ToLower();

        command.Parameters.Add(para);

        using var reader = await command.ExecuteReaderAsync();

        while (reader.Read())
        {
            countries.Add(new Country
            {
                Uuid = reader.GetString(reader.GetOrdinal("uuid")),
                Name = reader.GetString(reader.GetOrdinal("name")),
                Continent = reader.GetString(reader.GetOrdinal("continent")),
                SyncLevel = (SyncLevel)reader.GetInt32(reader.GetOrdinal("sync_level"))
            });
        }

        return countries;
    }

    public async Task<Country?> GetByNameAsync(string name)
    {
        Country? country = null;
        using var conn = _dbCon.CreateConnection();
        await conn.OpenAsync();

        const string query = "SELECT * FROM countries WHERE LOWER(name) = @countryName";

        using var command = conn.CreateCommand();
        command.CommandText = query;

        var para = command.CreateParameter();
        para.ParameterName = "countryName";
        para.Value = name.ToLower();

        command.Parameters.Add(para);

        using var reader = await command.ExecuteReaderAsync();

        if (reader.Read())
        {
            country = new Country
            {
                Uuid = reader.GetString(reader.GetOrdinal("uuid")),
                Name = reader.GetString(reader.GetOrdinal("name")),
                Continent = reader.GetString(reader.GetOrdinal("continent")),
                SyncLevel = (SyncLevel)reader.GetInt32(reader.GetOrdinal("sync_level"))
            };
        }

        return country;
    }

    public async Task<int> CreateBatchAsync(List<Country> countries)
    {
        using var conn = _dbCon.CreateConnection();
        await conn.OpenAsync();

        using var command = conn.CreateCommand();

        List<string> values = [];

        for (int i = 0; i < countries.Count; i++)
        {
            values.Add($"(@uuid_{i}, @name_{i}, @continent_{i}, @sync_level_{i})");

            var uuidPara = command.CreateParameter();
            uuidPara.ParameterName = $"@uuid_{i}";
            uuidPara.Value = countries[i].Uuid;
            command.Parameters.Add(uuidPara);

            var namePara = command.CreateParameter();
            namePara.ParameterName = $"@name_{i}";
            namePara.Value = countries[i].Name;
            command.Parameters.Add(namePara);


            var continentPara = command.CreateParameter();
            continentPara.ParameterName = $"@continent_{i}";
            continentPara.Value = countries[i].Continent;
            command.Parameters.Add(continentPara);


            var syncPara = command.CreateParameter();
            syncPara.ParameterName = $"@sync_level_{i}";
            syncPara.Value = countries[i].SyncLevel;
            command.Parameters.Add(syncPara);
        }

        string query = $@"
            INSERT INTO 
                countries (uuid, name, continent, sync_level) 
            VALUES
                {string.Join(", ", values)}";

        command.CommandText = query;

        return await command.ExecuteNonQueryAsync();
    }

    public async Task<bool> DeleteByContinentAsync(string continent)
    {
        using var conn = _dbCon.CreateConnection();
        await conn.OpenAsync();

        string query = $@"
            DELETE 
                FROM countries 
            WHERE 
                LOWER(continent) = @continentName AND
                sync_level = {(int)SyncLevel.country}
        ";

        using var command = conn.CreateCommand();
        command.CommandText = query;

        var para1 = command.CreateParameter();
        para1.ParameterName = "continentName";
        para1.Value = continent.ToLower();
        command.Parameters.Add(para1);

        using var reader = await command.ExecuteReaderAsync();

        return true;
    }

    public async Task<bool> DeleteAllAsync()
    {
        using var conn = _dbCon.CreateConnection();
        await conn.OpenAsync();

        const string query = "DELETE FROM countries";

        using var command = conn.CreateCommand();
        command.CommandText = query;

        using var reader = await command.ExecuteReaderAsync();

        return true;
    }

}