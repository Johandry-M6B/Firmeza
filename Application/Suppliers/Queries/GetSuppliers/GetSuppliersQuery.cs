// Firmeza.Application/Suppliers/Queries/GetSuppliers/GetSuppliersQuery.cs

using Application.Suppliers.DTOs;
using MediatR;

namespace Firmeza.Application.Suppliers.Queries.GetSuppliers;

public class GetSuppliersQuery : IRequest<IEnumerable<SupplierDto>>
{
    public bool OnlyActive { get; set; } = true;
    public string? SearchTerm { get; set; }
}