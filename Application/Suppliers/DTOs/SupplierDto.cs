// Firmeza.Application/Suppliers/DTOs/SupplierDto.cs
namespace Firmeza.Application.Suppliers.DTOs;

public class SupplierDto
{
    public int Id { get; set; }
    public string TradeName { get; set; } = string.Empty;
    public string Nit { get; set; } = string.Empty;
    public string? ContactName { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public bool Active { get; set; }
}