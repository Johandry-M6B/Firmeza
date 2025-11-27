using Domain.Enums;

namespace Application.Sales.DTOs;

public class PaymentSaleDto
{
    public int Id { get; set; }
    public DateTime PaymentDate { get; set; }
    public decimal Amount { get; set; }
    public PaymentFrom PaymentFrom { get; set; }
    public string? ReferenceNumber { get; set; }
    public string? Observations { get; set; }
}