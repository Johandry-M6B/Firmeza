using Domain.Entities;
using Domain.Interfaces;
using MediatR;

namespace Application.Suppliers.Commands.CreateSuppliers;

public class CreateSupplierCommandHandler : IRequestHandler<CreateSupplierCommand, int>
{
    private readonly ISupplierRepository _supplierRepository;

    public CreateSupplierCommandHandler(ISupplierRepository supplierRepository)
    {
        _supplierRepository = supplierRepository;
    }

    public async Task<int> Handle(CreateSupplierCommand request, CancellationToken cancellationToken)
    {
        var supplier = new Supplier
        {
            TradeName = request.TradeName,
            Nit = request.Nit,
            ContactName = request.ContactName,
            PhoneNumber = request.PhoneNumber,
            Email = request.Email,
            DateCreated = DateTime.UtcNow
        };

      var created =  await _supplierRepository.AddAsync(supplier);
        
        return created.Id;
    }
}