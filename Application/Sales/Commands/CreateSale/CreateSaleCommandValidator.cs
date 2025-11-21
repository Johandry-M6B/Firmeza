using FluentValidation;

namespace Application.Sales.Commands.CreateSale;

public class CreateSaleCommandValidator : AbstractValidator<CreateSaleCommand>
{
    public CreateSaleCommandValidator()
    {
        RuleFor(c => c.CustomerId)
            .GreaterThan(0).WithMessage("You must select a customer");
        
        RuleFor(d => d.Details)
            .NotEmpty().WithMessage("Details cannot be empty");
        RuleForEach(d => d.Details).ChildRules(detail =>
        {
            detail.RuleFor(d => d.ProductId)
                .GreaterThan(0).WithMessage("Product Invalid");
            
            detail.RuleFor(d => d.Quantity)
                .GreaterThan(0).WithMessage("The quantity must be greater than zero");
            
            detail.RuleFor(d => d.Price)
                .GreaterThan(0).WithMessage("The price must be greater than zero");
            
            detail.RuleFor(d => d.Discount)
                .GreaterThanOrEqualTo(0).WithMessage("The discount cannot be negative");
            
        });
        RuleFor(d => d.Discount)
            .GreaterThanOrEqualTo(0).WithMessage("The discount cannot be negative");

    }
}