using FluentValidation;

namespace Application.Products.Commands;

public class CreateProductCommandValidator :AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(p => p.Name)
            .NotEmpty().WithMessage("The code is required")
            .MaximumLength(20).WithMessage("The code cannot be longer than 20 character");

        RuleFor(p => p.Name)
            .NotEmpty().WithMessage("The name is required")
            .MaximumLength(100).WithMessage("The name cannot be longer than 100 character");

        RuleFor(p => p.Description)
            .MaximumLength(200).WithMessage("The description cannot be longer than 200 character ");

        RuleFor(p => p.CategoryId)
            .GreaterThan(0).WithMessage("You must select a valid category");

        RuleFor(p => p.MeasurementId)
            .GreaterThan(0).WithMessage("You must select a valid measurement");

        RuleFor(p => p.BuyerPrice)
            .GreaterThan(0).WithMessage("The Price of buyer must at 0");

        RuleFor(p => p.SalePrice)
            .GreaterThan(0).WithMessage("The price of sale must at 0")
            .GreaterThanOrEqualTo(p => p.BuyerPrice)
            .WithMessage("The sale price must be greater than or equal to the purchase price ");

        RuleFor(p => p.WholesalePrice)
            .GreaterThan(0).WithMessage("The price the for Wholesale must at 0 ");

        RuleFor(p => p.InitialStock)
            .GreaterThanOrEqualTo(0).WithMessage("The stock initial cannot are negative");

        RuleFor(p => p.MinimumStock)
            .GreaterThanOrEqualTo(0).WithMessage("The stock minimum cannot are negative");

        RuleFor(p => p.Weight)
            .GreaterThan(0).When(p => p.Weight.HasValue)
            .WithMessage("Weight must be greater than 0  ");
    }
}