using System.Net;
using System.Net.Mail;
using Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Firmeza.Infrastructure.Services;

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<EmailService> _logger;
    private readonly string _smtpServer;
    private readonly int _smtpPort;
    private readonly string _smtpUsername;
    private readonly string _smtpPassword;
    private readonly string _fromEmail;
    private readonly string _fromName;
    private readonly bool _enableSsl;

    public EmailService(
        IConfiguration configuration,
        ILogger<EmailService> logger)
    {
        _configuration = configuration;
        _logger = logger;
        
        // Leer configuración del appsettings.json
        _smtpServer = _configuration["Email:SmtpServer"] ?? "smtp.gmail.com";
        _smtpPort = int.Parse(_configuration["Email:SmtpPort"] ?? "587");
        _smtpUsername = _configuration["Email:Username"] ?? "";
        _smtpPassword = _configuration["Email:Password"] ?? "";
        _fromEmail = _configuration["Email:FromEmail"] ?? "";
        _fromName = _configuration["Email:FromName"] ?? "Firmeza";
        _enableSsl = bool.Parse(_configuration["Email:EnableSsl"] ?? "true");
    }

    public async Task SendEmailAsync(string to, string subject, string body)
    {
        try
        {
            using var client = new SmtpClient(_smtpServer, _smtpPort)
            {
                Credentials = new NetworkCredential(_smtpUsername, _smtpPassword),
                EnableSsl = _enableSsl,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Timeout = 30000 // 30 segundos
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_fromEmail, _fromName),
                Subject = subject,
                Body = body,
                IsBodyHtml = true,
                Priority = MailPriority.Normal
            };

            mailMessage.To.Add(to);

            _logger.LogInformation("Enviando email a {To} con asunto: {Subject}", to, subject);
            
            await client.SendMailAsync(mailMessage);
            
            _logger.LogInformation("Email enviado exitosamente a {To}", to);
        }
        catch (SmtpException ex)
        {
            _logger.LogError(ex, "Error SMTP al enviar email a {To}: {Message}", to, ex.Message);
            throw new InvalidOperationException($"Error al enviar email: {ex.Message}", ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error inesperado al enviar email a {To}", to);
            throw new InvalidOperationException($"Error inesperado al enviar email: {ex.Message}", ex);
        }
    }

    public async Task SendInvoiceEmailAsync(string to, int saleId, byte[] pdfContent)
    {
        try
        {
            using var client = new SmtpClient(_smtpServer, _smtpPort)
            {
                Credentials = new NetworkCredential(_smtpUsername, _smtpPassword),
                EnableSsl = _enableSsl,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Timeout = 30000
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_fromEmail, _fromName),
                Subject = $"Factura de Venta #{saleId} - Firmeza",
                IsBodyHtml = true,
                Priority = MailPriority.Normal
            };

            // Cuerpo del email con HTML profesional
            mailMessage.Body = GenerateInvoiceEmailBody(saleId);

            mailMessage.To.Add(to);

            // Adjuntar PDF
            using var pdfStream = new MemoryStream(pdfContent);
            var attachment = new Attachment(
                pdfStream, 
                $"Factura_{saleId}.pdf", 
                "application/pdf"
            );
            mailMessage.Attachments.Add(attachment);

            _logger.LogInformation("Enviando factura #{SaleId} a {To}", saleId, to);
            
            await client.SendMailAsync(mailMessage);
            
            _logger.LogInformation("Factura #{SaleId} enviada exitosamente a {To}", saleId, to);
        }
        catch (SmtpException ex)
        {
            _logger.LogError(ex, "Error SMTP al enviar factura #{SaleId} a {To}", saleId, to);
            throw new InvalidOperationException($"Error al enviar factura por email: {ex.Message}", ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error inesperado al enviar factura #{SaleId} a {To}", saleId, to);
            throw new InvalidOperationException($"Error inesperado al enviar factura: {ex.Message}", ex);
        }
    }

    private string GenerateInvoiceEmailBody(int saleId)
    {
        return $@"
<!DOCTYPE html>
<html lang='es'>
<head>
    <meta charset='UTF-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <title>Factura de Venta</title>
    <style>
        body {{
            font-family: Arial, sans-serif;
            line-height: 1.6;
            color: #333;
            max-width: 600px;
            margin: 0 auto;
            padding: 20px;
        }}
        .header {{
            background-color: #1e40af;
            color: white;
            padding: 20px;
            text-align: center;
            border-radius: 5px 5px 0 0;
        }}
        .content {{
            background-color: #f9fafb;
            padding: 30px;
            border: 1px solid #e5e7eb;
        }}
        .footer {{
            background-color: #f3f4f6;
            padding: 15px;
            text-align: center;
            font-size: 12px;
            color: #6b7280;
            border-radius: 0 0 5px 5px;
        }}
        .button {{
            display: inline-block;
            background-color: #1e40af;
            color: white;
            padding: 12px 30px;
            text-decoration: none;
            border-radius: 5px;
            margin: 20px 0;
        }}
        .important {{
            background-color: #fef3c7;
            border-left: 4px solid #f59e0b;
            padding: 15px;
            margin: 20px 0;
        }}
    </style>
</head>
<body>
    <div class='header'>
        <h1>FIRMEZA</h1>
        <p>Materiales de Construcción</p>
    </div>
    
    <div class='content'>
        <h2>Factura de Venta</h2>
        <p>Estimado cliente,</p>
        
        <p>Le enviamos adjunto la <strong>Factura #{saleId}</strong> correspondiente a su compra.</p>
        
        <div class='important'>
            <strong>📎 Documento adjunto:</strong> Factura_{saleId}.pdf
        </div>
        
        <p>Por favor, revise el documento adjunto para verificar los detalles de su compra.</p>
        
        <p>Si tiene alguna pregunta o inquietud sobre esta factura, no dude en contactarnos:</p>
        
        <ul>
            <li>📞 Teléfono: (605) 300-1234</li>
            <li>📧 Email: ventas@firmeza.com</li>
            <li>🏢 Dirección: Calle 123 #45-67, Barranquilla</li>
        </ul>
        
        <p><strong>Gracias por su preferencia.</strong></p>
        
        <p>Atentamente,<br>
        <strong>Equipo Firmeza</strong></p>
    </div>
    
    <div class='footer'>
        <p>Este es un correo electrónico automático, por favor no responda a esta dirección.</p>
        <p>© {DateTime.Now.Year} Firmeza - Materiales de Construcción. Todos los derechos reservados.</p>
    </div>
</body>
</html>
";
    }
}