namespace Domain.Exceptions;

public class SaleAlreadyPaidException : DomainException
{
    public int SaleId { get; }
    public string InvoiceNumber { get; }

    public SaleAlreadyPaidException(int saleId, string invoiceNumber)
        : base(
            $"Sale cannot be modified {invoiceNumber} because it has already been paid.",
            "SALE_ALREADY_PAID")
    {
        SaleId = saleId;
        InvoiceNumber = invoiceNumber;
        AddErrorDetail("SaleId", saleId);
        AddErrorDetail("InvoiceNumber", invoiceNumber);
    }
    
}