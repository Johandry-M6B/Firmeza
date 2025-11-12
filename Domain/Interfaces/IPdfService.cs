namespace Domain.Interfaces;

public interface IPdfService
{
    Task<byte[]> GenerateInvoicePdfAsync(int saleId);
    Task<byte[]> SendInvoiceEmailAsync(string to, int saleId, byte[] pdfContent);
}