using Domain.Enums;

namespace Application.Sales.DTOs;

public class SaleDto
{
    public int Id { get; set; }
    public string InvoiceNumber { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public int CustomerId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public decimal SubTotal { get; set; }
    public decimal Discount { get; set; }
    public decimal Vat { get; set; }
    public decimal Total { get; set; }
    public PaymentFrom PaymentFrom { get; set; }
    public SaleStatus Status { get; set; }
    public decimal AmountPaid { get; set; }
    public decimal Balance { get; set; }
    public DateTime? FullPaymentDate { get; set; }
    public string? DeliveryAddress { get; set; }
    public DateTime? DeliveryDate { get; set; }
    public bool Devoted { get; set; }
    public string? Observations { get; set; }
    public DateTime DateCreated { get; set; }
    
    public List<SaleDetailDto> SalesDetails { get; set; } = new();
    public List<PaymentSaleDto> Payments { get; set; } = new();
}