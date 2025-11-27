using Application.Sales.DTOs;
using AutoMapper;
using Domain.Interfaces;
using MediatR;

namespace Application.Sales.Queries.GetSales;

public class GetSalesQueryHandler : IRequestHandler<GetSalesQuery, IEnumerable<SaleDto>>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;

    public GetSalesQueryHandler(
        ISaleRepository saleRepository,
        IMapper mapper)
    {
        _saleRepository = saleRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<SaleDto>> Handle(GetSalesQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<Domain.Entities.Sale> sales;

        if (request.CustomerId.HasValue)
        {
            sales = await _saleRepository.GetByCustomerAsync(request.CustomerId.Value);
        }
        else if (request.Status.HasValue)
        {
            sales = await _saleRepository.GetByStatusAsync(request.Status.Value);
        }
        else if (request.StartDate.HasValue && request.EndDate.HasValue)
        {
            sales = await _saleRepository.GetByDateRangeAsync(request.StartDate.Value, request.EndDate.Value);
        }
        else
        {
            sales = await _saleRepository.GetAllAsync();
        }

        return _mapper.Map<IEnumerable<SaleDto>>(sales);
    } 
}