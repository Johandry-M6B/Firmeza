// Firmeza.Application/Products/Commands/UpdateProduct/UpdateProductCommandValidator.cs
using FluentValidation;

namespace Firmeza.Application.Products.Commands.UpdateProduct;

public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("El ID del producto es requerido");

        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("El código es requerido")
            .MaximumLength(20).WithMessage("El código no puede tener más de 20 caracteres");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("El nombre es requerido")
            .MaximumLength(100).WithMessage("El nombre no puede tener más de 100 caracteres");

        RuleFor(x => x.CategoryId)
            .GreaterThan(0).WithMessage("Debe seleccionar una categoría");

        RuleFor(x => x.MeasurementId)
            .GreaterThan(0).WithMessage("Debe seleccionar una unidad de medida");

        RuleFor(x => x.BuyerPrice)
            .GreaterThan(0).WithMessage("El precio de compra debe ser mayor a 0");

        RuleFor(x => x.SalePrice)
            .GreaterThan(0).WithMessage("El precio de venta debe ser mayor a 0")
            .GreaterThanOrEqualTo(x => x.BuyerPrice)
            .WithMessage("El precio de venta debe ser mayor o igual al precio de compra");

        RuleFor(x => x.WholesalePrice)
            .GreaterThan(0).WithMessage("El precio al por mayor debe ser mayor a 0");

        RuleFor(x => x.MinimumStock)
            .GreaterThanOrEqualTo(0).WithMessage("El stock mínimo no puede ser negativo");
    }
}