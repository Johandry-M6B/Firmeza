using Domain.Entities;
using Domain.Enums;
using Domain.Exceptions;
using Domain.Interfaces;
using MediatR;

namespace Application.Sales.Commands.CreateSale;

public class CreateSaleCommandHandler : IRequestHandler<CreateSaleCommand, int>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IProductRepository _productRepository;
    private readonly ICustomerRepository _customerRepository;

    public CreateSaleCommandHandler(
        ISaleRepository saleRepository,
        IProductRepository productRepository,
        ICustomerRepository customerRepository)
    {
        _saleRepository = saleRepository;
        _productRepository = productRepository;
        _customerRepository = customerRepository;
    }

    public async Task<int> Handle(CreateSaleCommand request, CancellationToken cancellationToken)
    {
        // Verificar que el cliente existe
        var customer = await _customerRepository.GetByIdAsync(request.CustomerId);
        if (customer == null)
        {
            throw new EntityNotFoundException(nameof(Customer), request.CustomerId);
        }

        // Generar número de factura (simplificado)
        var invoiceNumber = $"FAC-{DateTime.Now:yyyyMMddHHmmss}";

        // Crear la venta
        var sale = new Sale
        {
            InvoiceNumber = invoiceNumber,
            Date = DateTime.UtcNow,
            CustomerId = request.CustomerId,
            PaymentFrom = request.PaymentFrom,
            Status = SaleStatus.Pending,
            Discount = request.Discount,
            DeliveryAddress = request.DeliveryAddress,
            DeliveryDate = request.DeliveryDate,
            Observations = request.Observations,
            DateCreated = DateTime.UtcNow,
            SalesDetails = new List<SalesDetail>()
        };

        decimal subTotal = 0;
        decimal totalVat = 0;

        // Procesar cada detalle
        foreach (var detailDto in request.Details)
        {
            var product = await _productRepository.GetByIdAsync(detailDto.ProductId);
            if (product == null)
            {
                throw new EntityNotFoundException(nameof(Product), detailDto.ProductId);
            }

            // Verificar stock
            if (product.CurrentStock < detailDto.Quantity)
            {
                throw new InsufficientStockException(
                    detailDto.Quantity,
                    product.CurrentStock,
                    product.Code,
                    product.Name
                );
            }

            // Calcular totales
            var detailSubTotal = (detailDto.Price * detailDto.Quantity) - detailDto.Discount;
            var detailVat = detailSubTotal * 0.19m; // IVA 19%
            var detailTotal = detailSubTotal + detailVat;

            var detail = new SalesDetail
            {
                ProductId = detailDto.ProductId,
                Quantity = detailDto.Quantity,
                Price = detailDto.Price,
                Discount = detailDto.Discount,
                SubTotal = detailSubTotal,
                VatPercentage = 19,
                Vat = detailVat,
                Total = detailTotal,
                Observations = detailDto.Observations
            };

            sale.SalesDetails.Add(detail);

            subTotal += detailSubTotal;
            totalVat += detailVat;

            // Reducir stock
            product.ReduceStock(detailDto.Quantity);
            await _productRepository.UpdateAsync(product);
        }

        // Calcular totales de la venta
        sale.SubTotal = subTotal;
        sale.Vat = totalVat;
        sale.Total = subTotal + totalVat - request.Discount;
        sale.Balance = sale.Total;
        sale.AmountPaid = 0;

        // Guardar la venta
        var createdSale = await _saleRepository.AddAsync(sale);

        return createdSale.Id;
    }
}