namespace Domain.Exceptions;

public class InvalidDiscountException:DomainException
{
    public decimal AttemptedDiscount { get; }
    public decimal MaximumDiscount { get; }

    public InvalidDiscountException(decimal attemptedDiscount)
        : base(
            $"El descuento no puede ser negativo. Valor ingresado: {attemptedDiscount}%",
            "INVALID_DISCOUNT_NEGATIVE")
    {
        AttemptedDiscount = attemptedDiscount;
        AddErrorDetail("AttemptedDiscount", attemptedDiscount);
        AddErrorDetail("Reason", "NegativeDiscount");
    }

    public InvalidDiscountException(decimal attemptedDiscount, decimal maximumDiscount)
        : base(
            $"El descuento no puede exceder {maximumDiscount}%. Valor ingresado: {attemptedDiscount}%",
            "INVALID_DISCOUNT_EXCEEDED")
    {
        AttemptedDiscount = attemptedDiscount;
        MaximumDiscount = maximumDiscount;
        AddErrorDetail("AttemptedDiscount", attemptedDiscount);
        AddErrorDetail("MaximumDiscount", maximumDiscount);
        AddErrorDetail("Reason", "ExceededMaximum");
    }
}