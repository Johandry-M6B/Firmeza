namespace Domain.Exceptions;

public class DuplicateProductCodeException : DomainException
{
    public string DuplicateCode { get; }
    public int? ExistingProductId { get; }

    public DuplicateProductCodeException(string code)
        : base(
            $"Ya existe un producto con el código '{code}'. Los códigos deben ser únicos.",
            "DUPLICATE_PRODUCT_CODE")
    {
        DuplicateCode = code;
        AddErrorDetail("Code", code);
    }

    public DuplicateProductCodeException(string code, int existingProductId)
        : base(
            $"Ya existe un producto con el código '{code}' (ID: {existingProductId}). " +
            $"Los códigos deben ser únicos.",
            "DUPLICATE_PRODUCT_CODE")
    {
        DuplicateCode = code;
        ExistingProductId = existingProductId;
        AddErrorDetail("Code", code);
        AddErrorDetail("ExistingProductId", existingProductId);
    }
}