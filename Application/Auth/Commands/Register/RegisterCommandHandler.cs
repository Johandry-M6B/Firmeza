using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using MediatR;

namespace Application.Auth.Commands.Register;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, int>
{
    private readonly ICustomerRepository _customerRepository;

    public RegisterCommandHandler(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task<int> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var customer = new Customer
        {
            TypeCustomer = TypeCustomer.Retail,
            FullName = request.FullName,
            DocumentNumber = request.DocumentNumber,
            Email = request.Email,
            PhoneNumber = request.PhoneNumber,
            City = request.City,
            Country = request.Country,
            CreditLimit = 0,
            DaysToPay = 0,
            SpecialDiscount = null,
            Active = true,
            DateRegistered = DateTime.UtcNow
        };

        await _customerRepository.AddAsync(customer);
        return customer.Id;
    }
}
