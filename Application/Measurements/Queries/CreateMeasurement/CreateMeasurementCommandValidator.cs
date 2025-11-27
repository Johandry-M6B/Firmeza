using FluentValidation;

namespace Application.Measurements.Queries.CreateMeasurement;

public class CreateMeasurementCommandValidator : AbstractValidator<CreateMeasurementCommand>
{
    public CreateMeasurementCommandValidator()
    {
        RuleFor(m => m.Name)
            .NotEmpty().WithMessage("Name is required")
            .MaximumLength(50).WithMessage("Name cannot exceed 50 characters");
        
        RuleFor(m => m.Abbreviation)
            .NotEmpty().WithMessage("Abbreviation is required")
            .MaximumLength(10).WithMessage("Abbreviation cannot exceed 10 characters"); 
    }
}