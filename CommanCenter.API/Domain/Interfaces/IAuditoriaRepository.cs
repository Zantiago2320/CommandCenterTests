using CommanCenter.API.Domain.Entities;

namespace CommanCenter.API.Domain.Interfaces;

public interface IAuditoriaRepository : IRepository<AuditoriaLog>
{
    Task<IEnumerable<AuditoriaLog>> GetByModuloAsync(string modulo);
    Task<IEnumerable<AuditoriaLog>> GetByUsuarioAsync(string usuarioId);
    Task<IEnumerable<AuditoriaLog>> GetByFechaAsync(DateTime desde, DateTime hasta);
    Task<IEnumerable<AuditoriaLog>> GetRecientesAsync(int top = 100);
    
    /// <summary>Registra un cambio en la auditoría (versión heredada)</summary>
    Task RegistrarAsync(string modulo, string accion, string entidad,
        string? entidadId, string? valorAnterior, string? valorNuevo,
        string? usuarioId, string? usuarioEmail, string? ip, bool exitoso = true, string? error = null);

    /// <summary>Registra un cambio en la auditoría con todos los detalles</summary>
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
