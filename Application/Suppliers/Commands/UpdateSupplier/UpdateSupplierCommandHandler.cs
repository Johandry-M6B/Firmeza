using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces;
using MediatR;

namespace Application.Suppliers.Commands.UpdateSupplier;

public class UpdateSupplierCommandHandler : IRequestHandler<UpdateSupplierCommand, Unit>
{
    private readonly ISupplierRepository _supplierRepository;

    public UpdateSupplierCommandHandler(ISupplierRepository supplierRepository)
    {
        _supplierRepository = supplierRepository;
    }

    public async Task<Unit> Handle(UpdateSupplierCommand request, CancellationToken cancellationToken)
    {
        var supplier = await _supplierRepository.GetByIdAsync(request.Id);
        if (supplier == null)
        {
            throw new EntityNotFoundException(nameof(Supplier), request.Id);
        }
        
        supplier.TradeName = request.TradeName;
        supplier.Nit = request.Nit;
        supplier.ContactName = request.ContactName;
        supplier.Email = request.Email;
        supplier.Address = request.Address;
        supplier.City = request.City;
        supplier.PhoneNumber = request.PhoneNumber;
        
        await _supplierRepository.UpdateAsync(supplier);

        return Unit.Value;

    }
    
    
}