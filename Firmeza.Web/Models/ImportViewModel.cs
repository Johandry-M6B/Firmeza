using System.ComponentModel.DataAnnotations;

namespace Firmeza.Web.Models;

public class ImportProductsViewModel
{
    [Required(ErrorMessage = "You must select a file")]
    [Display(Name = "File Excel")]
    public IFormFile ExcelFile { get; set; } = null!;
    
    public List<ProductImportResult> Results { get; set; } = new();
    public int TotalProcessed { get; set; }
    public int TotalSuccess { get; set; }
    public int ErrorCount { get; set; }
}

public class ProductImportResult
{
    public int Row { get; set; }
    public string Code { get; set; } = string.Empty;
    public string ProductName { get; set; } = string.Empty;
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public string ErrorDetails { get; set; } = string.Empty;
}

public class ProductImportDto
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string CategoryName { get; set; } = string.Empty;
    public string MeasurementName { get; set; } = string.Empty;
    public string SupplierName { get; set; } = string.Empty;
    public decimal BuyerPrice { get; set; }
    public decimal SalePrice { get; set; }
    public decimal WholesalePrice { get; set; }
    public int CurrentStock { get; set; }
    public int MinimumStock { get; set; }
    public string? Mark { get; set; }
    public string? Model { get; set; }
    public string? Color { get; set; }
    public decimal? Weight { get; set; }
    public string? Size { get; set; }
    public bool RequiredRefrigeration { get; set; }
    public bool DangerousMaterial { get; set; }
}