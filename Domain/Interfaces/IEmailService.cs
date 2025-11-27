namespace Domain.Interfaces;

public interface IEmailService
{
    Task SendEmailAsync(string to,  string subject, string body);
    Task SendInvoiceEmailAsync(string to, int saleId, byte[] pdfContent);
}