using Microsoft.Data.SqlClient;

namespace DapperRetailSalesProject.Context
{
    public class DapperContext
    {
        private readonly string _connectionString;

        public DapperContext(IConfiguration configuration)
        {
            var cs = configuration.GetConnectionString("DefaultConnection");

            if (string.IsNullOrWhiteSpace(cs))
                throw new Exception("DefaultConnection BOŞ geliyor. appsettings.json okunmuyor ya da isim uyuşmuyor.");

            _connectionString = cs;
        }

        public SqlConnection CreateConnection()
            => new SqlConnection(_connectionString);
    }
}
