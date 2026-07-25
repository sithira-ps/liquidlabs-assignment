
using System.Text.Json;
using liquidlabs_assignment.Data;
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
        var countries = new List<Country>();
        using var conn = _dbCon.CreateConnection();
        conn.Open();

        const string query = "SELECT * FROM countries";

        using var command = conn.CreateCommand();
        command.CommandText = query;
        using var reader = command.ExecuteReader();

        while (reader.Read()) // returns true if there's a row to read or false when there are no more rows
        {
            countries.Add(new Country
            {
                Uuid = reader.GetString(reader.GetOrdinal("Id")),
                Name = reader.GetString(reader.GetOrdinal("Name")),
                Continent = reader.GetString(reader.GetOrdinal("Continent"))
            });
        }

        return countries;
    }

    public async Task<Country?> GetByNameAsync(string name)
    {
        Country? country = null;
        using var conn = _dbCon.CreateConnection();
        conn.Open();

        const string query = "SELECT * FROM countries WHERE name = @countryName";

        using var command = conn.CreateCommand();
        command.CommandText = query;

        var para = command.CreateParameter();
        para.ParameterName = "countryName";
        para.Value = "Sri Lanka";

        command.Parameters.Add(para);

        using var reader = command.ExecuteReader();

        if (reader.Read())
        {
            country = new Country
            {
                Uuid = reader.GetString(reader.GetOrdinal("Id")),
                Name = reader.GetString(reader.GetOrdinal("Name")),
                Continent = reader.GetString(reader.GetOrdinal("Continent"))
            };
        }

        return country;
    }
}