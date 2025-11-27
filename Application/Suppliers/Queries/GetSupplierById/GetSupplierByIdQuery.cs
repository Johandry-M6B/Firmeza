using Application.Suppliers.DTOs;
using MediatR;

namespace Application.Suppliers.Queries.GetSupplierById;

public class GetSupplierByIdQuery : IRequest<SupplierDto?>
{
    public int Id { get; set; }
    
    public GetSupplierByIdQuery(int id)
    {
        Id = id;
    }
}