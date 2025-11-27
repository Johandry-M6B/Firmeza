using FluentValidation;

namespace Application.Customers.Commands.CreateCustomer;

public class CreateCustomerCommandValidator : AbstractValidator<CreateCustomerCommand>
{
    public CreateCustomerCommandValidator()
    {
        RuleFor(c => c.FullName)
            .NotEmpty().WithMessage("Full Name is required")
            .MaximumLength(100).WithMessage("Full Name cannot exceed 100 characters");

        RuleFor(c => c.DocumentNumber)
            .NotEmpty().WithMessage("Document Number is required")
            .MaximumLength(20).WithMessage("Document Number cannot exceed 20 characters");
        
        RuleFor(c => c.Email)
            .EmailAddress().WithMessage("Invalid Email format")
            .When(c => !string.IsNullOrEmpty(c.Email));
        
        RuleFor(c => c.CreditLimit)
            .GreaterThanOrEqualTo(0).WithMessage("Credit Limit must be non-negative");

        RuleFor(c => c.DaysToPay)
            .GreaterThanOrEqualTo(0).WithMessage("Days to Pay must be non-negative");

        RuleFor(c => c.SpecialDiscount)
            .GreaterThanOrEqualTo(0).When(c => c.SpecialDiscount.HasValue)
            .WithMessage("Special Discount must be non-negative")
            .LessThanOrEqualTo(100).When(c => c.SpecialDiscount.HasValue)
            .WithMessage("Special Discount must be greater than or equal to 100%");
    }

}