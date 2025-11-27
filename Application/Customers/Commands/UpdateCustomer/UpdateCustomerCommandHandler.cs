using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces;
using MediatR;

namespace Application.Customers.Commands.UpdateCustomer;

public class UpdateCustomerCommandHandler : IRequestHandler<UpdateCustomerCommand, Unit>
{
    private readonly ICustomerRepository _customerRepository;

    public UpdateCustomerCommandHandler(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task<Unit> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
    {
        var customer = await _customerRepository.GetByIdAsync(request.Id);

        if (customer == null)
        {
            throw new EntityNotFoundException(nameof(Customer), request.Id);
        }
        var existingCustomer = await _customerRepository.GetByDocumentAsync(request.DocumentNumber);
        if (existingCustomer != null)
        {
            throw new InvalidOperationException($"A customer already exists with the document");
        }
        customer.TypeCustomer = request.TypeCustomer;
        customer.FullName = request.FullName;
        customer.Nit = request.Nit;
        customer.DocumentNumber = request.DocumentNumber;
        customer.PhoneNumber = request.PhoneNumber;
        customer.Email = request.Email;
        customer.City = request.City;
        customer.Country = request.Country;
        customer.CreditLimit = request.CreditLimit;
        customer.DaysToPay = request.DaysToPay;
        customer.SpecialDiscount = request.SpecialDiscount;
        
        await _customerRepository.UpdateAsync(customer);
        return Unit.Value;
    }
}