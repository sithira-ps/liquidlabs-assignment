
using System.Data;
using Microsoft.Data.SqlClient;

namespace liquidlabs_assignment.Data;

public class DbConnectionFactory : IDbConnectionFactory
{

    private readonly string _connectionString;

    public DbConnectionFactory(IConfiguration config)
    {
        _connectionString = config.GetConnectionString("DefaultConnection") ?? throw new Exception();
    }

    public IDbConnection CreateConnection()
    {
        return new SqlConnection(_connectionString);
    }
}