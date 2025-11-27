using FluentValidation;

namespace Application.Suppliers.Commands.CreateSuppliers;

public class CreateSupplierCommandValidator : AbstractValidator<CreateSupplierCommand>
{
    public CreateSupplierCommandValidator()
    {
        RuleFor(s => s.TradeName)
            .NotEmpty().WithMessage("Trade Name is required")
            .MaximumLength(100).WithMessage("Trade Name cannot exceed 100 characters");
        
        RuleFor(s => s.Nit)
            .NotEmpty().WithMessage("NIT is required")
            .MaximumLength(20).WithMessage("NIT cannot exceed 20 characters");
        
        RuleFor(s => s.PhoneNumber)
            .MaximumLength(15).WithMessage("Phone Number cannot exceed 15 characters");
        
        RuleFor(s => s.Email)
            .EmailAddress().WithMessage("Invalid Email format")
            .MaximumLength(100).WithMessage("Email cannot exceed 100 characters");
    }
}