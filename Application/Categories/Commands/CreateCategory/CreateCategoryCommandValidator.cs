using FluentValidation;
using FluentValidation.Validators;

namespace Application.Categories.Commands.CreateCategory;

public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryCommandValidator()
    {
        RuleFor(c => c.Name)
            .NotEmpty().WithMessage("Name is required")
            .MaximumLength(100).WithMessage("Name must not exceed 100 characters");
        
        RuleFor(c => c.Description)
            .MaximumLength(200).WithMessage("Description must not exceed 200 characters");
    }
}