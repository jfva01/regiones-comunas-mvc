using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PruebaTecnica.Web.Models;
using PruebaTecnica.Web.Services;

namespace PruebaTecnica.Web.Controllers;

public class HomeController : Controller
{
    private readonly IApiService _apiService;
    private readonly ILogger<HomeController> _logger;

    public HomeController(IApiService apiService, ILogger<HomeController> logger)
    {
        _apiService = apiService;
        _logger = logger;
    }

    public async Task<IActionResult> Index()
    {
        try
        {
           var regiones = await _apiService.ObtenerRegionesAsync();

            return View(regiones); 
        }
        catch(Exception ex)
        {
            _logger.LogError(ex, "Error al cargar las regiones.");

            return View(new List<RegionViewModel>());
        }
        
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    [HttpGet]
    public async Task<IActionResult> ObtenerComunasRegion(int idRegion)
    {
        try
        {
            var comunas = await _apiService.ObtenerComunasRegionAsync(idRegion);

            return Json(comunas);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener las comunas.");

            return Problem("Ocurrió un error al obtener las comunas.");
        }
    }

    [HttpGet]
    public async Task<IActionResult> ObtenerComuna(int idComuna)
    {
        try
        {
            var comuna = await _apiService.ObtenerComunaAsync(idComuna);

            if (comuna is null)
                return NotFound();

            return Json(comuna);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener la comuna.");

            return Problem("Ocurrió un error al obtener la comuna.");
        }
    }

    [HttpPut]
    public async Task<IActionResult> ActualizarComuna([FromBody] ComunaViewModel comuna)
    {
        try
        {
            await _apiService.ActualizarComunaAsync(comuna);

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al actualizar la comuna.");

            return Problem("Ocurrió un error al actualizar la comuna.");
        }
    }
}
