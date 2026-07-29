using System.Net.Http.Json;
using PruebaTecnica.Web.Models;

namespace PruebaTecnica.Web.Services;

public class ApiService : IApiService
{
    private readonly HttpClient _httpClient;

    public ApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<RegionViewModel>> ObtenerRegionesAsync()
    {
        var regiones = await _httpClient.GetFromJsonAsync<List<RegionViewModel>>("api/Region");

        return regiones ?? new List<RegionViewModel>();
    }

    public async Task<RegionViewModel?> ObtenerRegionAsync(int IdRegion)
    {
        return await _httpClient.GetFromJsonAsync<RegionViewModel>($"api/Region/{IdRegion}");
    }

    public async Task<List<ComunaViewModel>> ObtenerComunasRegionAsync(int IdRegion)
    {
        var comunas = await _httpClient.GetFromJsonAsync<List<ComunaViewModel>>(
            $"api/Region/{IdRegion}/comunas"
        );

        return comunas ?? new List<ComunaViewModel>();
    }

    public async Task<ComunaViewModel?> ObtenerComunaAsync(int IdComuna)
    {
        return await _httpClient.GetFromJsonAsync<ComunaViewModel>(
            $"api/Comuna/comuna/{IdComuna}"
        );
    }

    public async Task ActualizarComunaAsync(ComunaViewModel comuna)
    {
        var response = await _httpClient.PutAsJsonAsync(
            $"api/Comuna/{comuna.IdComuna}", comuna
        );

        response.EnsureSuccessStatusCode(); // lanzará una excepción si la API responde con un código de error (400, 404, 500)
    }
}