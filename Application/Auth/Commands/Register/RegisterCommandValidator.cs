using Domain.Interfaces;
using FluentValidation;

namespace Application.Auth.Commands.Register;

public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    private readonly ICustomerRepository _customerRepository;

    public RegisterCommandValidator(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;

        RuleFor(x => x.FullName)
            .NotEmpty().WithMessage("El nombre completo es requerido")
            .MaximumLength(200).WithMessage("El nombre no puede exceder 200 caracteres");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("El email es requerido")
            .EmailAddress().WithMessage("El formato del email no es válido")
            .MaximumLength(100).WithMessage("El email no puede exceder 100 caracteres")
            .MustAsync(BeUniqueEmail).WithMessage("Este email ya está registrado");

        RuleFor(x => x.DocumentNumber)
            .NotEmpty().WithMessage("El número de documento es requerido")
            .MaximumLength(50).WithMessage("El número de documento no puede exceder 50 caracteres");

        RuleFor(x => x.PhoneNumber)
            .MaximumLength(20).WithMessage("El teléfono no puede exceder 20 caracteres")
            .When(x => !string.IsNullOrEmpty(x.PhoneNumber));

        RuleFor(x => x.City)
            .MaximumLength(100).WithMessage("La ciudad no puede exceder 100 caracteres")
            .When(x => !string.IsNullOrEmpty(x.City));

        RuleFor(x => x.Country)
            .MaximumLength(100).WithMessage("El país no puede exceder 100 caracteres")
            .When(x => !string.IsNullOrEmpty(x.Country));
    }

    private async Task<bool> BeUniqueEmail(string email, CancellationToken cancellationToken)
    {
        var existingCustomer = await _customerRepository.GetByEmailAsync(email);
        return existingCustomer == null;
    }
}
