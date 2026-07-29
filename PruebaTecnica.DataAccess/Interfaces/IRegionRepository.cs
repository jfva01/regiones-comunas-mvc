using PruebaTecnica.DataAccess.Models;

namespace PruebaTecnica.DataAccess.Interfaces
{
    public interface IRegionRepository
    {
        Task<IEnumerable<Region>> ObtenerRegionesAsync();
        Task<Region?> ObtenerRegionAsync(int idRegion);
    }
}