using CommanCenter.API.Application.DTOs.Auditoria;
using CommanCenter.API.Application.DTOs.Common;
using CommanCenter.API.Application.Interfaces;
using CommanCenter.API.Domain.Entities;
using CommanCenter.API.Domain.Interfaces;

namespace CommanCenter.API.Application.Services;

public class AuditoriaService : IAuditoriaService
{
    private readonly IAuditoriaRepository _repo;

    public AuditoriaService(IAuditoriaRepository repo)
    {
        _repo = repo;
    }

    public async Task<ApiResponse<IEnumerable<AuditoriaLogDto>>> GetRecientesAsync(int top = 100)
    {
        if (top is < 1 or > 500) top = 100;
        var logs = await _repo.GetRecientesAsync(top);
        return ApiResponse<IEnumerable<AuditoriaLogDto>>.Ok(logs.Select(MapToDto));
    }

    public async Task<ApiResponse<IEnumerable<AuditoriaLogDto>>> GetByUsuarioAsync(string usuarioId)
    {
        var logs = await _repo.GetByUsuarioAsync(usuarioId);
        return ApiResponse<IEnumerable<AuditoriaLogDto>>.Ok(logs.Select(MapToDto));
    }

    public async Task<ApiResponse<IEnumerable<AuditoriaLogDto>>> GetByModuloAsync(string modulo)
    {
        var logs = await _repo.GetByModuloAsync(modulo);
        return ApiResponse<IEnumerable<AuditoriaLogDto>>.Ok(logs.Select(MapToDto));
    }

    public async Task<ApiResponse<IEnumerable<AuditoriaLogDto>>> GetByFechaAsync(DateTime desde, DateTime hasta)
    {
        var logs = await _repo.GetByFechaAsync(desde, hasta);
        return ApiResponse<IEnumerable<AuditoriaLogDto>>.Ok(logs.Select(MapToDto));
    }

    /// <summary>Registra un cambio en la auditoría</summary>
    public async Task RegistrarCambioAsync(
        string modulo,
        string accion,
        string entidad,
        string? entidadId,
        string? usuarioId,
        string? usuarioEmail,
        string? usuarioRol,
        string? campoModificado = null,
        string? valorAnterior = null,
        string? valorNuevo = null,
        string? razon = null
    )
    {
        try
        {
            await _repo.RegistrarCambioAsync(
                modulo, accion, entidad, entidadId, usuarioId, usuarioEmail, usuarioRol,
                campoModificado, valorAnterior, valorNuevo, razon
            );
        }
        catch
        {
            // No lanzar excepción - la auditoría no debe fallar la operación principal
        }
    }

    private static AuditoriaLogDto MapToDto(AuditoriaLog a) => new()
    {
        Id = a.Id,
        Fecha = a.FechaCreacion,
        Usuario = a.UsuarioId,
        UsuarioEmail = a.UsuarioEmail,
        UsuarioRol = a.UsuarioRol,
        IpAddress = a.IpAddress,
        UserAgent = a.UserAgent,
        Modulo = a.Modulo,
        Accion = a.Accion,
        Entidad = a.Entidad,
        EntidadId = a.EntidadId,
        CampoModificado = a.CampoModificado,
        ValorAnterior = a.ValorAnterior,
        ValorNuevo = a.ValorNuevo,
        Razon = a.Razon,
        Exitoso = a.Exitoso,
        Error = a.MensajeError
    };
}
