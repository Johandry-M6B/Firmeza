using MediatR;

namespace Application.Suppliers.Commands.CreateSuppliers;

public class CreateSupplierCommand : IRequest<int>
{
    public string TradeName { get; set; } = string.Empty;
    public string Nit { get; set; } = string.Empty;
    public string? ContactName { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
}