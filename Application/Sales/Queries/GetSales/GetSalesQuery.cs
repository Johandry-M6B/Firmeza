using Application.Sales.DTOs;
using Domain.Enums;
using MediatR;

namespace Application.Sales.Queries.GetSales;

public class GetSalesQuery : IRequest<IEnumerable<SaleDto>>
{
    public SaleStatus? Status { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public int? CustomerId { get; set; }
}