using MediatR;

namespace Application.Sales.Commands.CancelSale;

public class CancelSaleCommand :IRequest<Unit>
{
    public int SaleId { get; set; }
    public string Reason { get; set; } = string.Empty;
}