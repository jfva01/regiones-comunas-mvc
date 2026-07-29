using Microsoft.Data.SqlClient;

namespace PruebaTecnica.DataAccess.Database
{
    public interface ISqlConnectionFactory
    {
        SqlConnection CreateConnection();
    }
}