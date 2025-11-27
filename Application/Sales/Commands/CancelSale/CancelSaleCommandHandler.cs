using Domain.Entities;
using Domain.Enums;
using Domain.Exceptions;
using Domain.Interfaces;
using MediatR;

namespace Application.Sales.Commands.CancelSale;

public class CancelSaleCommandHandler : IRequestHandler<CancelSaleCommand, Unit>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IProductRepository _productRepository;

    public CancelSaleCommandHandler(
        ISaleRepository saleRepository,
        IProductRepository productRepository)
    {
        _saleRepository = saleRepository;
        _productRepository = productRepository;
    }

    public async Task<Unit> Handle(CancelSaleCommand request, CancellationToken cancellationToken)
    {
        var sale = await _saleRepository.GetByIdWithDetailsAsync(request.SaleId);

        if (sale == null)
        {
            throw new EntityNotFoundException(nameof(Sale), request.SaleId);
        }

        if (sale.Status == SaleStatus.Cancelled)
        {
            throw new InvalidOperationException("La venta ya está cancelada");
        }

        // Devolver stock de los productos
        foreach (var detail in sale.SalesDetails)
        {
            var product = await _productRepository.GetByIdAsync(detail.ProductId);
            if (product != null)
            {
                product.IncreaseStock(detail.Quantity);
                await _productRepository.UpdateAsync(product);
            }
        }

        // Cambiar estado de la venta
        sale.Status = SaleStatus.Cancelled;
        sale.Observations = $"{sale.Observations}\nCANCELADA: {request.Reason}";

        await _saleRepository.UpdateAsync(sale);

        return Unit.Value;
    }
}