using FluentValidation;

namespace Application.Measurements.Commands.UpdateMeasurements;

public class UpdateMeasurementCommandValidator : AbstractValidator<UpdateMeasurementCommand>
{
    public UpdateMeasurementCommandValidator()
    {
        RuleFor(m => m.Id)
            .GreaterThan(0).WithMessage("Id is required");
        
        RuleFor(m => m.Name)
            .NotEmpty().WithMessage("Name is required")
            .MaximumLength(50).WithMessage("Name cannot exceed 50 characters");
        
        RuleFor(m => m.Abbreviation)
            .NotEmpty().WithMessage("Abbreviation is required")
            .MaximumLength(10).WithMessage("Abbreviation cannot exceed 10 characters");
    }
}