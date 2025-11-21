using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces;
using MediatR;

namespace Application.Suppliers.Commands.DeleteSuppliers;

public class DeleteSupplierCommandHandler :IRequestHandler<DeleteSupplierCommand, Unit>
{
    private readonly ISupplierRepository _supplierRepository;

    public DeleteSupplierCommandHandler(ISupplierRepository supplierRepository)
    {
        _supplierRepository = supplierRepository;
    }

    public async Task<Unit> Handle(DeleteSupplierCommand request, CancellationToken cancellationToken)
    {
        var supplier = await _supplierRepository.GetByIdAsync(request.Id);

        if (supplier == null)
        {
            throw new EntityNotFoundException(nameof(Supplier), request.Id);
        }

        var hasProducts = await _supplierRepository.HasProductsAsync(request.Id);
        if (hasProducts)
        {
            throw new InvalidOperationException("Cannot delete supplier because it is associated with existing products.");
        }
            await _supplierRepository.DeleteAsync(request.Id);
            return Unit.Value;
    }
}