using Domain.Enums;
using MediatR;

namespace Application.Sales.Commands.AddPayment;

public class AddPaymentCommand : IRequest<Unit>
{
    public int SaleId { get; set; }
    public decimal Amount { get; set; }
    public PaymentFrom PaymentFrom { get; set; }
    public string? ReferenceNumber { get; set; }
    public string? Observations { get; set; }
}