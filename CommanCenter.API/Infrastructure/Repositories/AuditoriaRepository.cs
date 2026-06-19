using System.Security.Claims;
using CommanCenter.API.Domain.Entities;
using CommanCenter.API.Domain.Interfaces;
using CommanCenter.API.Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace CommanCenter.API.Infrastructure.Repositories;

public class AuditoriaRepository : Repository<AuditoriaLog>, IAuditoriaRepository
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuditoriaRepository(AppDbContext context, IHttpContextAccessor httpContextAccessor)
        : base(context)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<IEnumerable<AuditoriaLog>> GetByModuloAsync(string modulo) =>
        await _context.AuditoriaLogs
            .Where(a => a.Modulo == modulo)
            .OrderByDescending(a => a.FechaCreacion)
            .ToListAsync();

    public async Task<IEnumerable<AuditoriaLog>> GetByUsuarioAsync(string usuarioId) =>
        await _context.AuditoriaLogs
            .Where(a => a.UsuarioId == usuarioId)
            .OrderByDescending(a => a.FechaCreacion)
            .ToListAsync();

    public async Task<IEnumerable<AuditoriaLog>> GetByFechaAsync(DateTime desde, DateTime hasta) =>
        await _context.AuditoriaLogs
            .Where(a => a.FechaCreacion >= desde && a.FechaCreacion <= hasta)
            .OrderByDescending(a => a.FechaCreacion)
            .ToListAsync();

    public async Task<IEnumerable<AuditoriaLog>> GetRecientesAsync(int top = 100) =>
        await _context.AuditoriaLogs
            .OrderByDescending(a => a.FechaCreacion)
            .Take(top)
            .ToListAsync();

    public async Task RegistrarAsync(string modulo, string accion, string entidad,
        string? entidadId, string? valorAnterior, string? valorNuevo,
        string? usuarioId, string? usuarioEmail, string? ip,
        bool exitoso = true, string? error = null)
    {
        var http = _httpContextAccessor.HttpContext;
        var usuario = http?.User;

        // Si no llega información explícita, la tomo del contexto de la petición.
        usuarioId ??= usuario?.FindFirstValue(ClaimTypes.Name)
            ?? usuario?.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? "system";

        usuarioEmail ??= usuario?.FindFirstValue(ClaimTypes.Email)
            ?? usuario?.FindFirstValue("email")
            ?? usuarioId;

        ip ??= http?.Connection?.RemoteIpAddress?.ToString();
        var userAgent = http?.Request?.Headers["User-Agent"].ToString();

        try
        {
            var log = new AuditoriaLog
            {
                Modulo = modulo,
                Accion = accion,
                Entidad = entidad,
                EntidadId = entidadId,
                ValorAnterior = valorAnterior,
                ValorNuevo = valorNuevo,
                UsuarioId = usuarioId,
                UsuarioEmail = usuarioEmail,
                IpAddress = string.IsNullOrWhiteSpace(ip) ? "desconocida" : ip,
                UserAgent = string.IsNullOrWhiteSpace(userAgent) ? "desconocido" : userAgent,
                Exitoso = exitoso,
                MensajeError = error,
                FechaCreacion = DateTime.UtcNow
            };

            await _context.AuditoriaLogs.AddAsync(log);
            await _context.SaveChangesAsync();
        }
        catch
        {
            // La auditoría no debe interrumpir la operación principal
        }
    }

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
        var http = _httpContextAccessor.HttpContext;
        var usuario = http?.User;

        usuarioId ??= usuario?.FindFirstValue(ClaimTypes.Name)
            ?? usuario?.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? "system";

        usuarioEmail ??= usuario?.FindFirstValue(ClaimTypes.Email)
            ?? usuario?.FindFirstValue("email")
            ?? usuarioId;

        usuarioRol ??= usuario?.FindFirstValue(ClaimTypes.Role);

        var ip = http?.Connection?.RemoteIpAddress?.ToString();
        var userAgent = http?.Request?.Headers["User-Agent"].ToString();

        try
        {
            var log = new AuditoriaLog
            {
                Modulo = modulo,
                Accion = accion,
                Entidad = entidad,
                EntidadId = entidadId,
                UsuarioId = usuarioId,
                UsuarioEmail = usuarioEmail,
                UsuarioRol = usuarioRol,
                CampoModificado = campoModificado,
                ValorAnterior = valorAnterior,
                ValorNuevo = valorNuevo,
                Razon = razon,
                IpAddress = string.IsNullOrWhiteSpace(ip) ? "desconocida" : ip,
                UserAgent = string.IsNullOrWhiteSpace(userAgent) ? "desconocido" : userAgent,
                Exitoso = true,
                FechaCreacion = DateTime.UtcNow
            };

            await _context.AuditoriaLogs.AddAsync(log);
            await _context.SaveChangesAsync();
        }
        catch
        {
            // La auditoría no debe interrumpir la operación principal
        }
    }
}
