namespace Domain.Exceptions;

public class InvalidQuantityException : DomainException
{
    public int AttemptedQuantity { get; }

    public InvalidQuantityException(int quantity)
        : base(
            $"The quantity '{quantity}' is invalid. Quantity must be a positive integer.",
            "INVALID_QUANTITY_NEGATIVE")
    {
        AttemptedQuantity = quantity;
        AddErrorDetail("AttemptedQuantity", quantity);
    }
}
