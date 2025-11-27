// Firmeza.Application/Products/Commands/UpdateProduct/UpdateProductCommandHandler.cs

using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces;
using MediatR;

namespace Firmeza.Application.Products.Commands.UpdateProduct;

public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, Unit>
{
    private readonly IProductRepository _productRepository;

    public UpdateProductCommandHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<Unit> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        // Obtener el producto
        var product = await _productRepository.GetByIdAsync(request.Id);

        if (product == null)
        {
            throw new EntityNotFoundException(nameof(Product), request.Id);
        }

        // Actualizar información básica
        product.UpdateBasicInfo(
            name: request.Name,
            description: request.Description,
            categoryId: request.CategoryId,
            measurementId: request.MeasurementId,
            supplierId: request.SupplierId
        );

        // Actualizar precios
        product.UpadatePrices(
            buyerPrice: request.BuyerPrice,
            salePrice: request.SalePrice,
            wholesalePrice: request.WholesalePrice
        );

        // Actualizar características físicas
        product.UpdatePhysicalCharacteristics(
            mark: request.Mark,
            model: request.Model,
            color: request.Color,
            weight: request.Weight,
            size: request.Size,
            requiredRefrigeration: request.RequiredRefrigeration,
            dangerousMaterial: request.DangerousMaterial
        );

        // Actualizar stock mínimo
        product.UpdateMinimunStock(request.MinimumStock);

        // Guardar cambios
        await _productRepository.UpdateAsync(product);

        return Unit.Value;
    }
}