using System.ComponentModel;

namespace Domain.Exceptions;

public class ProductNotFoundException : DomainException
{
    public int? ProductId { get; }
    public string? ProductCode { get; }

    public ProductNotFoundException(int productId)
        : base(
            $"No se encontró el producto con ID {productId}",
            "PRODUCT_NOT_FOUND")
    {
        ProductId = productId;
        AddErrorDetail("ProductId", productId);
    }

    public ProductNotFoundException(string productCode, bool byCode)
        : base(
            $"No se encontró el producto con código '{productCode}'",
            "PRODUCT_NOT_FOUND")
    {
        ProductCode = productCode;
        AddErrorDetail("ProductCode", productCode);
    }
}