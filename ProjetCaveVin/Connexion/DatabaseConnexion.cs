using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace EvoTrackBack.Tools
{

    public interface ISqlConnectionFactory
    {
        IDbConnection CreateConnection();
    }


    public class SqlConnectionFactory(IConfiguration config) : ISqlConnectionFactory
    {
        private readonly string _defaultConnectionString =
                config.GetConnectionString("DefaultConnection")
                ?? throw new Exception("Connection string 'DefaultConnection' not found.");




        public IDbConnection CreateConnection()
        {
            return new SqlConnection(_defaultConnectionString);
        }
    }
}
