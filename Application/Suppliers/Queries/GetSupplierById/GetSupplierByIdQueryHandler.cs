using Application.Suppliers.DTOs;
using AutoMapper;
using Domain.Interfaces;
using MediatR;

namespace Application.Suppliers.Queries.GetSupplierById;

public class GetSupplierByIdQueryHandler : IRequestHandler<GetSupplierByIdQuery, SupplierDto?>
{
    private readonly IMapper _mapper;
    private readonly ISupplierRepository _supplierRepository;
    
    public GetSupplierByIdQueryHandler(
        ISupplierRepository supplierRepository,
        IMapper mapper)
    {
        _supplierRepository = supplierRepository;
        _mapper = mapper;
    }
    
    public async Task<SupplierDto?> Handle(GetSupplierByIdQuery request, CancellationToken cancellationToken)
    {
        var supplier = await _supplierRepository.GetByIdAsync(request.Id);
        return supplier == null ? null : _mapper.Map<SupplierDto>(supplier);
    }
}