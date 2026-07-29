using PruebaTecnica.DataAccess.Database;
using PruebaTecnica.DataAccess.Interfaces;
using PruebaTecnica.DataAccess.Models;
using System.Data;
using System.Xml.Linq;
using System.Globalization;

namespace PruebaTecnica.DataAccess.Repositories
{
    // Constructor
    public class ComunaRepository : IComunaRepository
    {
        private readonly ISqlConnectionFactory _connectionFactory;

        public ComunaRepository(ISqlConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        #region Funciones Públicas
        public async Task<IEnumerable<Comuna>> ObtenerComunasRegionAsync(int idRegion)
        {
            var comunas = new List<Comuna>();

            using var connection = _connectionFactory.CreateConnection();
            await connection.OpenAsync();

            using var command = connection.CreateCommand();
            command.CommandText = "sp_Listar_Comuna_Region";
            command.Parameters.Add("@idRegion", SqlDbType.Int).Value = idRegion; // Indicamos el tipo de datos para que ADO.NET no lo tenga que inferir
            command.CommandType = CommandType.StoredProcedure;

            using var reader = await command.ExecuteReaderAsync();

            // Buscar el indice de la columna una sola vez
            int idComunaOrdinal = reader.GetOrdinal("idComuna");
            int nombreOrdinal = reader.GetOrdinal("Comuna");
            int idRegionOrdinal = reader.GetOrdinal("idRegion");

            while (await reader.ReadAsync())
            {
                var comuna = new Comuna
                {
                    IdComuna = reader.GetInt32(idComunaOrdinal),
                    Nombre= reader.GetString(nombreOrdinal),
                    IdRegion= reader.GetInt32(idRegionOrdinal),
                    InformacionAdicional = ParsearInformacion( // Implementamos una función para parsear el Xml
                        reader["InformacionAdicional"].ToString()!)
                };

                comunas.Add(comuna);
            }

            return comunas;
        }

        public async Task<Comuna?> ObtenerComunaAsync(int idComuna)
        {
            using var connection = _connectionFactory.CreateConnection();
            await connection.OpenAsync();

            using var command = connection.CreateCommand();
            command.CommandText = "sp_Info_Comuna";
            command.Parameters.Add("@idComuna", SqlDbType.Int).Value = idComuna;
            command.CommandType = CommandType.StoredProcedure;

            using var reader = await command.ExecuteReaderAsync();

            if (!await reader.ReadAsync())
            {
                return null!;
            }

            // Buscar el indice de la columna una sola vez
            int idComunaOrdinal = reader.GetOrdinal("idComuna");
            int nombreOrdinal = reader.GetOrdinal("Comuna");
            int idRegionOrdinal = reader.GetOrdinal("idRegion");

            var comuna = new Comuna
            {
                IdComuna = reader.GetInt32(idComunaOrdinal),
                Nombre= reader.GetString(nombreOrdinal),
                IdRegion= reader.GetInt32(idRegionOrdinal),
                InformacionAdicional = ParsearInformacion( // Implementamos una función para parsear el Xml
                    reader["InformacionAdicional"].ToString()!)
            };

            return comuna;
        }

        public async Task ActualizarComunaAsync(Comuna comuna)
        {
            if (comuna is null)
                throw new ArgumentNullException(nameof(comuna));

            if (comuna.InformacionAdicional is null)
                throw new ArgumentNullException(nameof(comuna.InformacionAdicional));

            using var connection = _connectionFactory.CreateConnection();
            await connection.OpenAsync();

            using var command = connection.CreateCommand();
            command.CommandText = "sp_Actualiza_Comuna";
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@idComuna", SqlDbType.Int).Value = comuna.IdComuna;
            command.Parameters.Add("@Comuna", SqlDbType.NVarChar, 128).Value = comuna.Nombre;
            command.Parameters.Add("@InformacionAdicional", SqlDbType.Xml).Value = GenerarXml(comuna.InformacionAdicional); // Implementamos una función para crear el Xml
            command.Parameters.Add("@idRegion", SqlDbType.Int).Value = comuna.IdRegion;

            await command.ExecuteNonQueryAsync();

        }

        #endregion

        #region Funciones Privadas

        private InformacionAdicional ParsearInformacion(string xml)
        {// Función para leer el Xml
            var document = XDocument.Parse(xml);

            var root = document.Root
                ?? throw new InvalidOperationException("XML inválido.");

            var poblacionElement = root.Element("Poblacion")
                ?? throw new InvalidOperationException("El XML no contiene población.");

            return new InformacionAdicional
            {
                Superficie = decimal.Parse(
                    root.Element("Superficie")?.Value ?? "0",
                    CultureInfo.InvariantCulture),

                Poblacion = int.Parse(
                    poblacionElement.Value),

                Densidad = decimal.Parse(
                    poblacionElement.Attribute("Densidad")?.Value.Replace(",", ".") ?? "0",
                    CultureInfo.InvariantCulture)
            };
        }

        private string GenerarXml(InformacionAdicional infoAdicional)
        {// Función para crear el Xml
            var xml = new XElement("Info",
                new XElement("Superficie", infoAdicional.Superficie),
                new XElement(
                    "Poblacion",
                    new XAttribute(
                        "Densidad",
                        infoAdicional.Densidad.ToString(CultureInfo.InvariantCulture)),
                    infoAdicional.Poblacion));

            return xml.ToString(SaveOptions.DisableFormatting); // El Xml queda formateado en una sola línea
        }

        #endregion
    }
}