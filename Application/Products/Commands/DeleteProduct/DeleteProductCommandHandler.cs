// Firmeza.Application/Products/Commands/DeleteProduct/DeleteProductCommandHandler.cs

using Application.Products.Commands.DeleteProduct;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces;
using MediatR;


namespace Firmeza.Application.Products.Commands.DeleteProduct;

public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, Unit>
{
    private readonly IProductRepository _productRepository;

    public DeleteProductCommandHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<Unit> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(request.Id);

        if (product == null)
        {
            throw new EntityNotFoundException(nameof(Product), request.Id);
        }

        // En lugar de eliminar físicamente, desactivar el producto
        product.Deactivate();
        await _productRepository.UpdateAsync(product);

        return Unit.Value;
    }
}