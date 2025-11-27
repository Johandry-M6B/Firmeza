namespace Application.Sales.DTOs;

public class SaleDetailDto
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public decimal Discount { get; set; }
    public decimal SubTotal { get; set; }
    public decimal VatPercentage { get; set; }
    public decimal Vat { get; set; }
    public decimal Total { get; set; }
    public string? Observations { get; set; }
}