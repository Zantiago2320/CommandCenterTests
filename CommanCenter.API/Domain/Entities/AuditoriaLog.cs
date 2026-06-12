using CommanCenter.API.Domain.Common;

namespace CommanCenter.API.Domain.Entities;

/// <summary>
/// Auditoría centralizada. Aplica a TODOS los módulos del Command Center.
/// Registro INMUTABLE de todos los cambios en el sistema.
/// </summary>
public class AuditoriaLog : BaseEntity
{
    public string Modulo { get; set; } = string.Empty;               // DataTeam, RRHH, etc.
    public string Accion { get; set; } = string.Empty;               // CREATE, UPDATE, DELETE, DISABLE, ENABLE, LOGIN
    public string Entidad { get; set; } = string.Empty;              // Consultor, Celula, Usuario, etc.
    public string? EntidadId { get; set; }                           // ID del registro afectado
    public string? CampoModificado { get; set; }                     // Nombre del campo que cambió
    public string? ValorAnterior { get; set; }                       // Valor antes del cambio
    public string? ValorNuevo { get; set; }                          // Valor después del cambio
    public string? UsuarioId { get; set; }                           // ID del usuario que hizo el cambio
    public string? UsuarioEmail { get; set; }                        // Email del usuario que hizo el cambio
    public string? UsuarioRol { get; set; }                          // Rol del usuario (Admin, Supervisor, Senior)
    public string? Razon { get; set; }                               // Razón del cambio (especialmente para deshabilitaciones)
    public string? IpAddress { get; set; }
    public string? UserAgent { get; set; }
    public bool Exitoso { get; set; } = true;
    public string? MensajeError { get; set; }
}
