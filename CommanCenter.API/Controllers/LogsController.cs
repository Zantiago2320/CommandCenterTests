using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace CommanCenter.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin,Supervisor")]
[Produces("application/json")]
public class LogsController : ControllerBase
{
    private readonly ILogger<LogsController> _logger;
    private readonly string _logsDirectory;

    public LogsController(ILogger<LogsController> logger, IWebHostEnvironment env)
    {
        _logger = logger;
        _logsDirectory = Path.Combine(env.ContentRootPath, "Logs");
    }

    /// <summary>Obtiene los logs del día actual.</summary>
    [HttpGet("today")]
    public IActionResult GetTodayLogs()
    {
        try
        {
            var today = DateTime.UtcNow.ToString("yyyyMMdd");
            var logFile = Path.Combine(_logsDirectory, $"commancenter-{today}.log");

            if (!System.IO.File.Exists(logFile))
                return NotFound(new { exitoso = false, mensaje = $"No hay logs para hoy ({today})" });

            var logs = System.IO.File.ReadAllLines(logFile);
            return Ok(new
            {
                exitoso = true,
                mensaje = "Logs obtenidos exitosamente",
                data = new
                {
                    fecha = today,
                    cantidad = logs.Length,
                    logs = logs.TakeLast(100) // Últimas 100 líneas para no sobrecargar
                }
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener logs de hoy");
            return BadRequest(new { exitoso = false, mensaje = ex.Message });
        }
    }

    /// <summary>Obtiene los logs de una fecha específica (formato: YYYY-MM-DD).</summary>
    [HttpGet("date/{date}")]
    public IActionResult GetLogsByDate(string date)
    {
        try
        {
            if (!DateTime.TryParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsedDate))
                return BadRequest(new { exitoso = false, mensaje = "Formato de fecha inválido. Use: yyyy-MM-dd" });

            var dateStr = parsedDate.ToString("yyyyMMdd");
            var logFile = Path.Combine(_logsDirectory, $"commancenter-{dateStr}.log");

            if (!System.IO.File.Exists(logFile))
                return NotFound(new { exitoso = false, mensaje = $"No hay logs para la fecha {date}" });

            var logs = System.IO.File.ReadAllLines(logFile);
            return Ok(new
            {
                exitoso = true,
                mensaje = "Logs obtenidos exitosamente",
                data = new
                {
                    fecha = date,
                    cantidad = logs.Length,
                    logs = logs.TakeLast(100)
                }
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener logs de fecha {Date}", date);
            return BadRequest(new { exitoso = false, mensaje = ex.Message });
        }
    }

    /// <summary>Obtiene la lista de archivos de logs disponibles.</summary>
    [HttpGet("available")]
    public IActionResult GetAvailableLogs()
    {
        try
        {
            if (!Directory.Exists(_logsDirectory))
                return Ok(new
                {
                    exitoso = true,
                    mensaje = "No hay archivos de logs",
                    data = new { archivos = new List<object>() }
                });

            var files = Directory.GetFiles(_logsDirectory, "commancenter-*.log")
                .Select(f => new FileInfo(f))
                .OrderByDescending(f => f.LastWriteTime)
                .Select(f => new
                {
                    nombre = f.Name,
                    tamaño_kb = Math.Round(f.Length / 1024.0, 2),
                    ultima_modificacion = f.LastWriteTime
                })
                .ToList();

            return Ok(new
            {
                exitoso = true,
                mensaje = "Archivos de logs obtenidos",
                data = new { archivos = files }
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener lista de logs");
            return BadRequest(new { exitoso = false, mensaje = ex.Message });
        }
    }

    /// <summary>Obtiene logs filtrados por nivel (Error, Warning, Information).</summary>
    [HttpGet("level/{level}")]
    public IActionResult GetLogsByLevel(string level)
    {
        try
        {
            var validLevels = new[] { "Error", "Warning", "Information", "Debug" };
            if (!validLevels.Contains(level, StringComparer.OrdinalIgnoreCase))
                return BadRequest(new { exitoso = false, mensaje = $"Nivel inválido. Use: {string.Join(", ", validLevels)}" });

            var today = DateTime.UtcNow.ToString("yyyyMMdd");
            var logFile = Path.Combine(_logsDirectory, $"commancenter-{today}.log");

            if (!System.IO.File.Exists(logFile))
                return NotFound(new { exitoso = false, mensaje = "No hay logs para hoy" });

            var logs = System.IO.File.ReadAllLines(logFile)
                .Where(l => l.Contains(level, StringComparison.OrdinalIgnoreCase))
                .TakeLast(100)
                .ToList();

            return Ok(new
            {
                exitoso = true,
                mensaje = $"Logs de nivel '{level}' obtenidos",
                data = new
                {
                    fecha = today,
                    nivel = level,
                    cantidad = logs.Count,
                    logs = logs
                }
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al filtrar logs por nivel");
            return BadRequest(new { exitoso = false, mensaje = ex.Message });
        }
    }
}
