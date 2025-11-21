using Application.Customers.DTOs;
using AutoMapper;
using Domain.Interfaces;
using MediatR;

namespace Application.Customers.Queries.GetCustomers;

public class GetCustomersQueryHandler : IRequestHandler<GetCustomersQuery, IEnumerable<CustomerDto>>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IMapper _mapper;

    public GetCustomersQueryHandler(
        ICustomerRepository customerRepository,
        IMapper mapper)
    {
        _customerRepository = customerRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<CustomerDto>> Handle(GetCustomersQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<Domain.Entities.Customer> customers;

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            customers = await _customerRepository.SearchAsync(request.SearchTerm);
        }
        else if (request.Type.HasValue)
        {
            customers = await _customerRepository.GetByTypeAsync(request.Type.Value);
        }
        else if (request.OnlyActive)
        {
            customers = await _customerRepository.GetActiveAsync();
        }
        else
        {
            customers = await _customerRepository.GetAllAsync();
        }

        return _mapper.Map<IEnumerable<CustomerDto>>(customers);
    }
}