namespace Domain.Exceptions;

public class InvalidPriceException : DomainException
{
    public decimal AttemptedPrice { get; }
    public decimal? MinimumPrice { get; }
    public decimal? MaximumPrice { get; }
    public string? ProductName { get; }

    /// <summary>
    ///Contructor de precios negativos
    /// </summary>
    public InvalidPriceException(decimal attemptedPrice)
        : base(
            $"The price '{attemptedPrice}' is invalid because it cannot be negative.",
            "INVALID_PRICE_NEGATIVE")
    {
        AttemptedPrice = attemptedPrice;
        AddDetail("AttemptedPrice", attemptedPrice);
        AddDetail("Reason", "NegativePrice");
    }

    
}