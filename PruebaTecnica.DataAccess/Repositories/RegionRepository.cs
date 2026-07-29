using PruebaTecnica.DataAccess.Database;
using PruebaTecnica.DataAccess.Interfaces;
using PruebaTecnica.DataAccess.Models;
using System.Data;

namespace PruebaTecnica.DataAccess.Repositories
{
    // Constructor
    public class RegionRepository : IRegionRepository
    {
        private readonly ISqlConnectionFactory _connectionFactory;

        public RegionRepository(ISqlConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<IEnumerable<Region>> ObtenerRegionesAsync()
        {
            var regiones = new List<Region>();

            using var connection = _connectionFactory.CreateConnection();
            await connection.OpenAsync();

            using var command = connection.CreateCommand();
            command.CommandText = "sp_Region_Listar";
            command.CommandType = CommandType.StoredProcedure;

            using var reader = await command.ExecuteReaderAsync();
            
            // Buscar el indice de la columna una sola vez
            int idRegionOrdinal = reader.GetOrdinal("idRegion");
            int nombreOrdinal = reader.GetOrdinal("Region");

            while (await reader.ReadAsync())
            {
                var region = new Region
                {
                    IdRegion = reader.GetInt32(idRegionOrdinal),
                    Nombre= reader.GetString(nombreOrdinal)
                };

                regiones.Add(region);
            }

            return regiones;
        }

        public async Task<Region?> ObtenerRegionAsync(int idRegion)
        {
            using var connection = _connectionFactory.CreateConnection();
            await connection.OpenAsync();

            using var command = connection.CreateCommand();
            command.CommandText = "sp_Info_Region";
            command.Parameters.Add("@idRegion", SqlDbType.Int).Value = idRegion;
            command.CommandType = CommandType.StoredProcedure;

            using var reader = await command.ExecuteReaderAsync();

            // Buscar el indice de la columna una sola vez
            int idRegionOrdinal = reader.GetOrdinal("idRegion");
            int nombreOrdinal = reader.GetOrdinal("Region");

            if (!await reader.ReadAsync())
            {
                return null!;
            }

            var region = new Region
            {
                IdRegion = reader.GetInt32(idRegionOrdinal),
                Nombre= reader.GetString(nombreOrdinal)
            };

            return region;
        }
    }
}