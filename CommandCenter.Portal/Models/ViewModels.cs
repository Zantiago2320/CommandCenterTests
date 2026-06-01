namespace CommandCenter.Portal.Models;

public class CelulaViewModel
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public string Color { get; set; } = "#28a745";
    public string? ImagenUrl { get; set; }
    public bool Activo { get; set; }
    public int TotalMiembros { get; set; }
    public string? NombreLider { get; set; }
    public DateTime FechaCreacion { get; set; }
    public List<MiembroCelulaViewModel> Miembros { get; set; } = [];
}

public class MiembroCelulaViewModel
{
    public int ConsultorId { get; set; }
    public string NombreCompleto { get; set; } = string.Empty;
    public string? Cargo { get; set; }
    public bool EsLider { get; set; }
}

public class ConsultorViewModel
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Apellido { get; set; } = string.Empty;
    public string NombreCompleto => $"{Nombre} {Apellido}";
    public string Email { get; set; } = string.Empty;
    public string? Telefono { get; set; }
    public string? Cargo { get; set; }
    public string? Tecnologia { get; set; }
    public string? NivelSeniority { get; set; }
    public DateTime? FechaIngreso { get; set; }
    public bool Habilitado { get; set; }
    public string? FotoUrl { get; set; }
    public string? CelulaNombre { get; set; }
}
