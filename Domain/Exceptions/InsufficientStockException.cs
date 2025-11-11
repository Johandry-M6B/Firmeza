namespace Domain.Exceptions;

public class InsufficientStockException : DomainException
{
    public int RequestedQuantity { get; }
    public int AvailableStock { get; }
    public string? ProductCode { get; }
    public string? ProductName { get; }

    public InsufficientStockException(
        int requestedQuantity,
        int availableStock,
        string productCode,
        string productName)
        : base(
            $"Stock insuficiente para el producto '{productName}' (código: {productCode}). " +
            $"Solicitado: {requestedQuantity}, Disponible: {availableStock}",
            "INSUFFICIENT_STOCK")
    {
        RequestedQuantity = requestedQuantity;
        AvailableStock = availableStock;
        ProductCode = productCode;
        ProductName = productName;

        AddErrorDetail("RequestedQuantity", requestedQuantity);
        AddErrorDetail("AvailableStock", availableStock);
        AddErrorDetail("ProductCode", productCode);
        AddErrorDetail("ProductName", productName);
        AddErrorDetail("Shortage", requestedQuantity - availableStock);
    }

    /// <summary>
    /// Constructor simplificado cuando no hay nada de stock
    /// </summary>
    public InsufficientStockException(string productName)
        : base(
            $"El producto '{productName}' no tiene stock disponible",
            "NO_STOCK")
    {
        RequestedQuantity = 1;
        AvailableStock = 0;
        ProductName = productName;

        AddErrorDetail("ProductName", productName);
        AddErrorDetail("Reason", "NoStock");
    }
}