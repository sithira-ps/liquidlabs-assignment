using System.Data;
using System.Data.Common;

public interface IDbConnectionFactory
{
    IDbConnection CreateConnection();
}