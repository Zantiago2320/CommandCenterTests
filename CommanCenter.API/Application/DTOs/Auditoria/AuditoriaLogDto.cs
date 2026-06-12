namespace CommanCenter.API.Application.DTOs.Auditoria;

public class AuditoriaLogDto
{
    public int Id { get; set; }
    public DateTime Fecha { get; set; }
    public string? Usuario { get; set; }
    public string? UsuarioEmail { get; set; }
    public string? UsuarioRol { get; set; }
    public string? IpAddress { get; set; }
    public string? UserAgent { get; set; }
    public string Modulo { get; set; } = string.Empty;
    public string Accion { get; set; } = string.Empty;
    public string Entidad { get; set; } = string.Empty;
    public string? EntidadId { get; set; }
    public string? CampoModificado { get; set; }
    public string? ValorAnterior { get; set; }
    public string? ValorNuevo { get; set; }
    public string? Razon { get; set; }
    public bool Exitoso { get; set; }
    public string? Error { get; set; }
}
