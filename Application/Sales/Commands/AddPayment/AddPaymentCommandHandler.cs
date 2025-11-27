using Domain.Entities;
using Domain.Enums;
using Domain.Exceptions;
using Domain.Interfaces;
using MediatR;

namespace Application.Sales.Commands.AddPayment;

public class AddPaymentCommandHandler : IRequestHandler<AddPaymentCommand, Unit>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IPaymentSaleRepository _paymentSaleRepository;

    public AddPaymentCommandHandler(
        ISaleRepository saleRepository,
        IPaymentSaleRepository paymentSaleRepository)
    {
        _saleRepository = saleRepository;
        _paymentSaleRepository = paymentSaleRepository;
    }

    public async Task<Unit> Handle(AddPaymentCommand request, CancellationToken cancellationToken)
    {
        var sale = await _saleRepository.GetByIdAsync(request.SaleId);

        if (sale == null)
        {
            throw new EntityNotFoundException(nameof(Sale), request.SaleId);
        }

        if (sale.Status == SaleStatus.Cancelled)
        {
            throw new InvalidOperationException("No se puede agregar un pago a una venta cancelada");
        }

        if (request.Amount <= 0)
        {
            throw new InvalidOperationException("El monto del pago debe ser mayor a 0");
        }

        if (request.Amount > sale.Balance)
        {
            throw new InvalidOperationException($"El monto del pago ({request.Amount:C}) no puede ser mayor al saldo pendiente ({sale.Balance:C})");
        }

        // Crear el pago
        var payment = new PaymentSale
        {
            SaleId = request.SaleId,
            PaymentDate = DateTime.UtcNow,
            Amount = request.Amount,
            PaymentFrom = request.PaymentFrom,
            ReferenceNumber = request.ReferenceNumber,
            Observations = request.Observations,
            DateCreated = DateTime.UtcNow
        };
        await _paymentSaleRepository.AddAsync(payment);

        // Actualizar montos de la venta
        sale.AmountPaid += request.Amount;
        sale.Balance -= request.Amount;

        // Actualizar estado
        if (sale.Balance == 0)
        {
            sale.Status = SaleStatus.Paid

        ;
            sale.FullPaymentDate = DateTime.UtcNow;
        }
        else if (sale.AmountPaid > 0 && sale.Balance > 0)
        {
            sale.Status = SaleStatus.Pending;
        }

        await _saleRepository.UpdateAsync(sale);

        return Unit.Value;
    }
}

