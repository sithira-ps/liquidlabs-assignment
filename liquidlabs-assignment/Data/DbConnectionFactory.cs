
using System.Data.Common;
using Microsoft.Data.SqlClient;

namespace liquidlabs_assignment.Data;

public class DbConnectionFactory : IDbConnectionFactory
{

    private readonly string _connectionString;

    public DbConnectionFactory(IConfiguration config)
    {
        _connectionString = config.GetConnectionString("DefaultConnection") ?? throw new Exception("Connection string is not found.");
    }

    public DbConnection CreateConnection()
    {
        return new SqlConnection(_connectionString);
    }
}