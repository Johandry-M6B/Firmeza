using FluentValidation;
using FluentValidation.Validators;

namespace Application.Customers.Commands.UpdateCustomer;

public class UpdateCustomerCommandValidator : AbstractValidator<UpdateCustomerCommand>
{
    public UpdateCustomerCommandValidator()
    {
        RuleFor(c => c.Id)
            .GreaterThan(0).WithMessage("Id is required");
        
        RuleFor(c => c.FullName)
            .NotEmpty().WithMessage("Full Name is required")
            .MaximumLength(200).WithMessage("Full Name cannot exceed 200 characters");
        
        RuleFor(c => c.DocumentNumber)
            .NotEmpty().WithMessage("Document Number is required")
            .MaximumLength(50).WithMessage("Document Number cannot exceed 50 characters");
        
        RuleFor(c => c.TypeCustomer)
            .IsInEnum().WithMessage("Customer Type must be one of the following: Customer Type");
        
        RuleFor(c => c.Email)
            .EmailAddress().WithMessage("Invalid Email format")
            .MaximumLength(100).WithMessage("Email cannot exceed 100 characters");
        
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