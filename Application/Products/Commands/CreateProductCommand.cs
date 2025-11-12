using MediatR;

namespace Application.Products.Commands;

public class CreateProductCommand : IRequest<int>
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int CategoryId { get; set; }
    public int MeasurementId { get; set; }
    public int? SupplierId { get; set; }
    public decimal BuyerPrice { get; set; }
    public decimal SalePrice { get; set; }
    public decimal WholesalePrice { get; set; }
    public int InitialStock { get; set; }
    public int MinimumStock { get; set; }
    public string? Mark { get; set; }
    public string? Model { get; set; }
    public string? Color { get; set; }
    public decimal? Weight { get; set; }
    public string? Size { get; set; }
    public bool RequiredRefrigeration { get; set; }
    public bool DangerousMaterial { get; set; }
}