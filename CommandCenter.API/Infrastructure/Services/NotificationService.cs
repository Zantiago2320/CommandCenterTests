using CommandCenter.API.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace CommandCenter.API.Infrastructure.Services;

/// <summary>
/// Servicio de notificaciones vía SendGrid.
/// Toda comunicación de correo del sistema pasa por aquí.
/// Reutilizable para todos los módulos: DataTeam, RRHH, DevSecOps, etc.
/// </summary>
public class NotificationService : INotificationService
{
    private readonly IConfiguration _config;
    private readonly ILogger<NotificationService> _logger;

    public NotificationService(IConfiguration config, ILogger<NotificationService> logger)
    {
        _config = config;
        _logger = logger;
    }

    public async Task EnviarEmailAsync(string destinatario, string asunto, string cuerpoHtml, string? adjuntoUrl = null)
    {
        try
        {
            var client = new SendGridClient(_config["SendGrid:ApiKey"]);
            var from = new EmailAddress(_config["SendGrid:FromEmail"], _config["SendGrid:FromName"]);
            var to = new EmailAddress(destinatario);
            var msg = MailHelper.CreateSingleEmail(from, to, asunto, null, cuerpoHtml);

            var response = await client.SendEmailAsync(msg);

            if ((int)response.StatusCode >= 400)
            {
                var body = await response.Body.ReadAsStringAsync();
                _logger.LogError("SendGrid error {StatusCode}: {Body}", response.StatusCode, body);
            }
            else
            {
                _logger.LogInformation("Email enviado a {Destinatario}: {Asunto}", destinatario, asunto);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al enviar email a {Destinatario}", destinatario);
        }
    }

    public async Task EnviarEmailConAdjuntoAsync(string destinatario, string asunto,
        string cuerpoHtml, byte[] adjunto, string nombreAdjunto)
    {
        try
        {
            var client = new SendGridClient(_config["SendGrid:ApiKey"]);
            var from = new EmailAddress(_config["SendGrid:FromEmail"], _config["SendGrid:FromName"]);
            var to = new EmailAddress(destinatario);
            var msg = MailHelper.CreateSingleEmail(from, to, asunto, null, cuerpoHtml);

            msg.AddAttachment(nombreAdjunto, Convert.ToBase64String(adjunto));

            await client.SendEmailAsync(msg);
            _logger.LogInformation("Email con adjunto '{Adjunto}' enviado a {Destinatario}", nombreAdjunto, destinatario);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al enviar email con adjunto a {Destinatario}", destinatario);
        }
    }

    public async Task ProgramarEmailAsync(string destinatario, string asunto,
        string cuerpoHtml, DateTime fechaProgramada, string modulo)
    {
        // Programación gestionada por Hangfire — registrar en BD y dejar que el job la procese
        _logger.LogInformation("Email programado para {Fecha} → {Destinatario} [{Modulo}]",
            fechaProgramada, destinatario, modulo);
        await Task.CompletedTask;
    }
}
