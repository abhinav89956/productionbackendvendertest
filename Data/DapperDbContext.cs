using Microsoft.Data.SqlClient;
using System.Data;

namespace VenderTest.Data
{
    public class DapperDbContext
    {
        private readonly IConfiguration _config;

        public DapperDbContext(IConfiguration config)
        {
            _config = config;
        }

        public IDbConnection CreateConnection()
        {
            try
            {
                return new SqlConnection(
                    _config.GetConnectionString("DefaultConnection"));
            }
            catch (Exception ex)
            {
                throw new Exception("Database connection failed", ex);
            }
        }
    }
}
