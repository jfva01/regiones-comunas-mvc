using Microsoft.AspNetCore.Mvc;
using PruebaTecnica.DataAccess.Interfaces;

namespace PruebaTecnica.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RegionController : ControllerBase
{
    private readonly IRegionRepository _regionRepository;
    private readonly IComunaRepository _comunaRepository;
    private readonly ILogger<RegionController> _logger;

    public RegionController(IRegionRepository regionRepository, IComunaRepository comunaRepository, ILogger<RegionController> logger)
    {
        _regionRepository = regionRepository;
        _comunaRepository = comunaRepository;
        _logger = logger;
    }
    /// <summary>
    /// Obtiene todas las regiones.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> ObtenerRegionesAsync()
    {
        try
        {
            var regiones = await _regionRepository.ObtenerRegionesAsync();

            return Ok(regiones);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener las regiones");

            return StatusCode(StatusCodes.Status500InternalServerError,"Ocurrió un error interno");
        }
    }
    /// <summary>
    /// Obtiene una región según su Id.
    /// </summary>
    [HttpGet("{IdRegion}")]
    public async Task<IActionResult> ObtenerRegionAsync(int IdRegion)
    {
        var region = await _regionRepository.ObtenerRegionAsync(IdRegion);

        if(region is null)
        {
            _logger.LogError("Error al obtener la región");

            return NotFound();
        }

        return Ok(region);  
    }
    /// <summary>
    /// Obtiene las comunas de una región.
    /// </summary>
    [HttpGet("{IdRegion}/comunas")]
    public async Task<IActionResult> ObtenerComunasRegionAsync(int IdRegion)
    {
        try
        {
            var comunas = await _comunaRepository.ObtenerComunasRegionAsync(IdRegion);

            return Ok(comunas);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener las comunas de la región " + IdRegion);

            return StatusCode(StatusCodes.Status500InternalServerError,"Ocurrió un error interno");
        } 
    }
}