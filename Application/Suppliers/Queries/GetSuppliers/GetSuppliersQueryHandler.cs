// Firmeza.Application/Suppliers/Queries/GetSuppliers/GetSuppliersQueryHandler.cs

using AutoMapper;
using Domain.Interfaces;
using Firmeza.Application.Suppliers.DTOs;
using Firmeza.Application.Suppliers.Queries.GetSuppliers;
using MediatR;

namespace Application.Suppliers.Queries.GetSuppliers;

public class GetSuppliersQueryHandler : IRequestHandler<GetSuppliersQuery, IEnumerable<SupplierDto>>
{
    private readonly ISupplierRepository _supplierRepository;
    private readonly IMapper _mapper;

    public GetSuppliersQueryHandler(
        ISupplierRepository supplierRepository,
        IMapper mapper)
    {
        _supplierRepository = supplierRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<SupplierDto>> Handle(GetSuppliersQuery request, CancellationToken cancellationToken)
    {
        var suppliers = request.OnlyActive
            ? await _supplierRepository.GetActiveAsync()
            : await _supplierRepository.GetAllAsync();

        return _mapper.Map<IEnumerable<SupplierDto>>(suppliers);
    }
}