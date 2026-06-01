using CommandCenter.API.Application.DTOs.Consultores;
using CommandCenter.API.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CommandCenter.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
[Produces("application/json")]
public class ConsultoresController : ControllerBase
{
    private readonly IConsultorService _service;

    public ConsultoresController(IConsultorService service)
    {
        _service = service;
    }

    /// <summary>Obtiene todos los consultores habilitados.</summary>
    [HttpGet]
    [Authorize(Roles = "SuperAdmin,Admin,Lider,User")]
    public async Task<IActionResult> GetAll() =>
        Ok(await _service.GetAllAsync());

    /// <summary>Obtiene un consultor por ID.</summary>
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _service.GetByIdAsync(id);
        return result.Exitoso ? Ok(result) : NotFound(result);
    }

    /// <summary>Obtiene consultores de una célula.</summary>
    [HttpGet("celula/{celulaId:int}")]
    public async Task<IActionResult> GetByCelula(int celulaId) =>
        Ok(await _service.GetByCelulaAsync(celulaId));

    /// <summary>Crea un nuevo consultor.</summary>
    [HttpPost]
    [Authorize(Roles = "SuperAdmin,Admin")]
    public async Task<IActionResult> Crear([FromBody] CrearConsultorDto dto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "system";
        var result = await _service.CrearAsync(dto, userId);
        return result.Exitoso ? CreatedAtAction(nameof(GetById), new { id = result.Data?.Id }, result) : BadRequest(result);
    }

    /// <summary>Actualiza un consultor existente.</summary>
    [HttpPut("{id:int}")]
    [Authorize(Roles = "SuperAdmin,Admin")]
    public async Task<IActionResult> Actualizar(int id, [FromBody] ActualizarConsultorDto dto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "system";
        var result = await _service.ActualizarAsync(id, dto, userId);
        return result.Exitoso ? Ok(result) : BadRequest(result);
    }

    /// <summary>Deshabilita un consultor (soft disable).</summary>
    [HttpPatch("{id:int}/deshabilitar")]
    [Authorize(Roles = "SuperAdmin,Admin")]
    public async Task<IActionResult> Deshabilitar(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "system";
        var result = await _service.DeshabilitarAsync(id, userId);
        return result.Exitoso ? Ok(result) : NotFound(result);
    }

    /// <summary>Elimina lógicamente un consultor.</summary>
    [HttpDelete("{id:int}")]
    [Authorize(Roles = "SuperAdmin")]
    public async Task<IActionResult> Eliminar(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "system";
        var result = await _service.EliminarAsync(id, userId);
        return result.Exitoso ? Ok(result) : NotFound(result);
    }
}
