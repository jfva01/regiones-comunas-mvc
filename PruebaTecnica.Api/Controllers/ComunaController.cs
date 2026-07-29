using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using PruebaTecnica.DataAccess.Interfaces;
using PruebaTecnica.DataAccess.Models;

namespace PruebaTecnica.Api.Controllers;

[ApiController]
[Route("api/[controller]")]

public class ComunaController : ControllerBase
{
    private readonly IComunaRepository _repository;
    private readonly ILogger<ComunaController> _logger;

    public ComunaController(IComunaRepository repository, ILogger<ComunaController> logger)
    {
        _repository = repository;
        _logger = logger;
    }
    /// <summary>
    /// Obtiene una comuna según su Id.
    /// </summary>
    [HttpGet("comuna/{IdComuna}")]
    public async Task<IActionResult> ObtenerComunaAsync(int IdComuna)
    {
        var comuna = await _repository.ObtenerComunaAsync(IdComuna);

        if(comuna is null)
        {
            _logger.LogError("Error al obtener la comuna");

            return NotFound();
        }

        return Ok(comuna);  
    }
    /// <summary>
    /// Actualiza la información de una comuna.
    /// </summary>
    [HttpPut("{IdComuna}")]
    public async Task<IActionResult> ActualizarComunaAsync(int IdComuna, [FromBody] Comuna comuna)
    {
        if (IdComuna != comuna.IdComuna)
            return BadRequest("El id de la URL no coincide con el de la comuna.");

        try
        {
            await _repository.ActualizarComunaAsync(comuna);

            return NoContent();
        }
        catch (SqlException ex) when (ex.Number == 50001)
        {
            return NotFound(ex.Message);
        }
        catch(Exception ex)
        {
            _logger.LogError(ex, "Error al actualizar la comuna.");

            return StatusCode(StatusCodes.Status500InternalServerError,"Ocurrió un error interno.");
        }
    }
}