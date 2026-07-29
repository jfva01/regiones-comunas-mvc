using PruebaTecnica.DataAccess.Models;

namespace PruebaTecnica.DataAccess.Interfaces
{
    public interface IComunaRepository
    {
        Task<IEnumerable<Comuna>> ObtenerComunasRegionAsync(int idRegion);
        Task<Comuna?> ObtenerComunaAsync(int idComuna);
        Task ActualizarComunaAsync(Comuna comuna);
    }
}