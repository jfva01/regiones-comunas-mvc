using PruebaTecnica.Web.Models;

namespace PruebaTecnica.Web.Services
{
    public interface IApiService
    {
        Task<List<RegionViewModel>> ObtenerRegionesAsync();
        Task<RegionViewModel?> ObtenerRegionAsync(int idRegion);
        Task<List<ComunaViewModel>> ObtenerComunasRegionAsync(int idRegion);
        Task<ComunaViewModel?> ObtenerComunaAsync(int idComuna);
        Task ActualizarComunaAsync(ComunaViewModel comuna);
    }
}