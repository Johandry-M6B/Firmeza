// Firmeza.Application/Suppliers/Queries/GetSuppliers/GetSuppliersQuery.cs
using MediatR;
using Firmeza.Application.Suppliers.DTOs;

namespace Firmeza.Application.Suppliers.Queries.GetSuppliers;

public class GetSuppliersQuery : IRequest<IEnumerable<SupplierDto>>
{
    public bool OnlyActive { get; set; } = true;
}