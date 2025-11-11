using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net.NetworkInformation;
using Domain.Exceptions;
using Firmeza.Web.Data.Entities;

namespace Domain.Entities;

public abstract class Product
{
    [Key] public int Id { get; private set; }
    [Required] [MaxLength(20)] public string Code { get; private set; }
    [Required] [MaxLength(100)] public string Name { get; private set; }
    [MaxLength(500)] public string Description { get; private set; }
    [Required] public int CategoryId { get; private set; }
    [ForeignKey(nameof(CategoryId))] public virtual Category Category { get; private set; }
    [Required] public int MeasurementId { get; private set; }
    [ForeignKey(nameof(MeasurementId))] public virtual Measurement Measurement { get; private set; }

    public int? SupplierId { get; private set; }
    [ForeignKey(nameof(SupplierId))] public virtual Supplier Supplier { get; private set; }

    //Price e Inventory
    private decimal _buyerPrice;

    [Column(TypeName = "decimal(18,2)")]
    public decimal BuyerPrice
    {
        get => _buyerPrice;
        private set
        {
            if (value < 0 || value == 0)
                throw new InvalidPriceException(value);

            _buyerPrice = value;
        }
    }

    private decimal _salePrice;

    [Column(TypeName = "decimal(18,2)")]
    public decimal SalePrice
    {
        get => _salePrice;
        private set
        {
            if (value < 0 || value == 0)
                throw new InvalidPriceException(value);
            if (_buyerPrice > 0 && value < _buyerPrice)
                throw new InvalidPriceException(value, _buyerPrice, Name ?? "Product");

            _salePrice = value;
        }
    }

    private decimal _wholesalePrice;

    [Column(TypeName = "decimal(18,2)")]
    public decimal WholesalePrice
    {
        get => _wholesalePrice;
        private set
        {
            if (value < 0 || value == 0)
                throw new InvalidPriceException(value);

            _wholesalePrice = value;
        }
    }

    public int CurrentStock { get; private set; }
    public int MinimumStock { get; private set; }

    [MaxLength(50)] public string? Mark { get; private set; }
    [MaxLength(100)] public string? Model { get; private set; }
    [MaxLength(50)] public string? Color { get; private set; }
    [Column(TypeName = "decimal(18,2)")] public decimal? Weight { get; private set; } // in Kg
    [MaxLength(50)] public string? Size { get; private set; } // E.g: 2m x 1m

    //Control
    public bool RequiredRefrigeration { get; private set; } = false;
    public bool DangerousMaterial { get; private set; } = false;
    public bool Active { get; private set; } = true;
    public DateTime DateCreated { get; private set; } = DateTime.UtcNow;
    public DateTime? DateUpdated { get; private set; }

    public virtual ICollection<SalesDetail> SalesDetails { get; private set; } = new List<SalesDetail>();

    public virtual ICollection<InventoryMovement> InventoryMovements { get; private set; } =
        new List<InventoryMovement>();

    private Product()
    {
    }

    public Product(
        string code,
        string name,
        string? description,
        int categoryId,
        int measurementId,
        decimal buyerPrice,
        decimal salePrice,
        decimal wholesalePrice,
        int initialStock,
        int minimumStock,
        int? supplierId = null)
    {
        if (string.IsNullOrWhiteSpace(code))
            throw new ArgumentException("Code cannot be null or empty", nameof(code));
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be null or empty", nameof(name));
        if (initialStock < 0)
            throw new InvalidQuantityException(initialStock);
        if (minimumStock < 0)
            throw new InvalidQuantityException(minimumStock);

        Code = code;
        Name = name;
        Description = description;
        CategoryId = categoryId;
        MeasurementId = measurementId;
        SupplierId = supplierId;
        BuyerPrice = buyerPrice;
        SalePrice = salePrice;
        WholesalePrice = wholesalePrice;
        CurrentStock = initialStock;
        MinimumStock = minimumStock;
        Active = true;
        DateCreated = DateTime.UtcNow;
    }

    public void ReduceStock(int quantity)
    {
        if (quantity <= 0)
            throw new InvalidQuantityException(quantity);
        if (quantity > CurrentStock)
            throw new InsufficientStockException(quantity, CurrentStock, Code, Name);

        CurrentStock -= quantity;
        DateUpdated = DateTime.UtcNow;
    }

    public void IncreaseStock(int quantity)
    {
        if (quantity <= 0)
            throw new InvalidQuantityException(quantity);

        CurrentStock += quantity;
        DateUpdated = DateTime.UtcNow;
    }

    public void UpdateStock(int quantityChange)
    {
        var newStock = CurrentStock + quantityChange;

        if (newStock < 0)
            throw new InsufficientStockException(
                Math.Abs(quantityChange),
                CurrentStock,
                Code,
                Name);

        CurrentStock = newStock;
        DateUpdated = DateTime.UtcNow;
    }

    public void UpadatePrices(decimal buyerPrice, decimal salePrice, decimal wholesalePrice)
    {
        BuyerPrice = buyerPrice;
        SalePrice = salePrice;
        WholesalePrice = wholesalePrice;
        DateUpdated = DateTime.UtcNow;
    }

    public void UpdateBasicInfo(
        string name,
        string? description,
        int categoryId,
        int measurementId,
        int? supplierId = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("The name cannot have empty", nameof(name));
        Name = name;
        Description = description;
        CategoryId = categoryId;
        MeasurementId = measurementId;
        SupplierId = supplierId;
        DateUpdated = DateTime.UtcNow;
    }

    public void UpdatePhysicalCharacteristics(
        string? mark,
        string? model,
        string? color,
        decimal? weight,
        string? size,
        bool requiredRefrigeration,
        bool dangerousMaterial)
    {
        Mark = mark;
        Model = model;
        Color = color;
        Weight = weight;
        Size = size;
        RequiredRefrigeration = requiredRefrigeration;
        DangerousMaterial = dangerousMaterial;
        DateUpdated = DateTime.UtcNow;
    }

    public void UpdateMinimunStock(int minimumStock)
    {
        if (minimumStock < 0)
            throw new InvalidQuantityException(minimumStock);

        MinimumStock = minimumStock;
        DateUpdated = DateTime.UtcNow;

    }

    public void Deactivate()
    {
        Active = false;
        DateUpdated = DateTime.UtcNow;
    }

    public void Activate()
    {
        Active = true;
        DateUpdated = DateTime.UtcNow;
    }

    public bool IsLowStock()
    {
        return CurrentStock <= MinimumStock;
    }

    private bool HasStock()
    {
        return CurrentStock > 0;
    }

    private bool HasSufficientStock(int quantity)
    {
        return CurrentStock >= quantity;
    }
}

