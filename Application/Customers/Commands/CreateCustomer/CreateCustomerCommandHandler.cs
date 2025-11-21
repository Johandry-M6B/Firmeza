using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces;
using MediatR;

namespace Application.Customers.Commands.CreateCustomer;

public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, int>
{
    private readonly ICustomerRepository _customerRepository;
    
    public CreateCustomerCommandHandler(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task<int> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        if (await _customerRepository.ExistsDocumentAsync(request.DocumentNumber))
        {
            throw new InvalidOperationException($"A customer already exists with the document");
        }

        var customer = new Customer
        {
            TypeCustomer = request.TypeCustomer,
            FullName = request.FullName,
            Nit = request.Nit,
            DocumentNumber = request.DocumentNumber,
            PhoneNumber = request.PhoneNumber,
            Email = request.Email,
            City = request.City,
            Country = request.Country,
            CreditLimit = request.CreditLimit,
            DaysToPay = request.DaysToPay,
            SpecialDiscount = request.SpecialDiscount,
            Active = true,
            DateRegistered = DateTime.UtcNow
        };
        var created = await _customerRepository.AddAsync(customer);
        return customer.Id;
    }
}