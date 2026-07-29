using Microsoft.Data.SqlClient;

namespace PruebaTecnica.DataAccess.Database
{
    public class SqlConnectionFactory : ISqlConnectionFactory
    {
        private readonly string _connectionString;

        public SqlConnectionFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        public SqlConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}

// Jorge Vargas:
// Esta clase es responsable de crear instancias de SqlConnection 
// utilizando la cadena de conexión proporcionada.
// No debe ejecutar consultas ni conocer procedimientos almacenados.