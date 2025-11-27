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
        AddErrorDetail("AttemptedPrice", attemptedPrice);
        AddErrorDetail("Reason", "NegativePrice");
    }

    public InvalidPriceException(decimal attemptedPrice, decimal minPrice, decimal maxPrice)
        : base(
            $"The price '{attemptedPrice:No}' is invalid because it must be between '{minPrice:NO}' and '{maxPrice:NO}'.",
            "INVALID_PRICE_OUT_OF_RANGE")
    {
        AttemptedPrice = attemptedPrice;
        MinimumPrice = minPrice;
        MaximumPrice = maxPrice;
        AddErrorDetail("AttemptedPrice", attemptedPrice);
        AddErrorDetail("MinimumPrice", minPrice);
        AddErrorDetail("MaximumPrice", maxPrice);
        AddErrorDetail("Reason", "OutOfRange");
    }

    public InvalidPriceException(decimal salePrice, decimal buyPrice, string productName)
        : base($"The sale price (${salePrice:NO}) cannot be less than the purchase price(${buyPrice:NO})" +
               $"For the product '{productName}'. this would generate losses",
            "INVALID_PRICE_LOSS")
    {
        AttemptedPrice = salePrice;
        MinimumPrice = buyPrice;
        ProductName = productName;
        AddErrorDetail("AttemptedPrice", salePrice);
        AddErrorDetail("MinimumPrice", buyPrice);
        AddErrorDetail("ProductName", productName);
        AddErrorDetail("Reason", "SalePriceBelowbBuyerPrice");
    }

    public InvalidPriceException(decimal attemptedPrice, string customerMessage, string errorCode = "INVALID_PRICE")
        : base(customerMessage, errorCode)
    {
        AttemptedPrice = attemptedPrice;
        AddErrorDetail("AttemptedPrice" , attemptedPrice);
    }    
}