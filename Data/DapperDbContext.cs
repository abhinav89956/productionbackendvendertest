using Npgsql;
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
                return new NpgsqlConnection(
                    _config.GetConnectionString("DefaultConnection"));
            }
            catch (Exception ex)
            {
                throw new Exception("Database connection failed", ex);
            }
        }
    }
}
