namespace Application.Products.DTOs;

public class ProductDto
{
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int CategoryId { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public int MeasurementId { get; set; }
    public string MeasurementName { get; set; } = string.Empty;
    public int? SupplierId { get; set; }
    public string? SupplierName { get; set; }
    public decimal BuyerPrice { get; set; }
    public decimal SalePrice { get; set; }
    public decimal WholesalePrice { get; set; }
    public int CurrentStock { get; set; }
    public int MinimumStock { get; set; }
    public bool IsLowStock { get; set; }
    public string? Mark { get; set; }
    public string? Model { get; set; }
    public string? Color { get; set; }
    public decimal? Weight { get; set; }
    public string? Size { get; set; }
    public bool RequiredRefrigeration { get; set; }
    public bool DangerousMaterial { get; set; }
    public bool Active { get; set; }
    public DateTime DateCreated { get; set; }
    public DateTime? DateUpdated { get; set; }
}