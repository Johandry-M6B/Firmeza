using Application.Sales.DTOs;
using MediatR;

namespace Application.Sales.Queries.GetSaleById;

public class GetSaleByIdQuery : IRequest<SaleDto?>
{
    public int Id { get; set; }
    
    public GetSaleByIdQuery(int id)
    {
        Id = id;
    }
}