using CommanCenter.API.Application.DTOs.Auditoria;
using CommanCenter.API.Application.DTOs.Common;

namespace CommanCenter.API.Application.Interfaces;

public interface IAuditoriaService
{
    Task<ApiResponse<IEnumerable<AuditoriaLogDto>>> GetRecientesAsync(int top = 100);
    Task<ApiResponse<IEnumerable<AuditoriaLogDto>>> GetByUsuarioAsync(string usuarioId);
    Task<ApiResponse<IEnumerable<AuditoriaLogDto>>> GetByModuloAsync(string modulo);
    Task<ApiResponse<IEnumerable<AuditoriaLogDto>>> GetByFechaAsync(DateTime desde, DateTime hasta);

    /// <summary>Registra un cambio en la auditoría</summary>
    Task RegistrarCambioAsync(
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
    );
}
