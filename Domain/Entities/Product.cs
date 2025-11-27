using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net.NetworkInformation;
using Domain.Exceptions;

namespace Domain.Entities;

public  class Product
{
     public int Id { get; private set; }
     public string Code { get; set; }
      public string Name { get; set; }
    public string Description { get; set; }
    public int CategoryId { get; set; }
    public Category Category { get; private set; }
     public int MeasurementId { get; set; }
   public Measurement Measurement { get; private set; }

    public int? SupplierId { get; set; }
     public Supplier Supplier { get; private set; }

    //Price e Inventory
    private decimal _buyerPrice;

   
    public decimal BuyerPrice
    {
        get => _buyerPrice;
        set
        {
            if (value < 0 || value == 0)
                throw new InvalidPriceException(value);

            _buyerPrice = value;
        }
    }

    private decimal _salePrice;

    
    public decimal SalePrice
    {
        get => _salePrice;
        set
        {
            if (value < 0 || value == 0)
                throw new InvalidPriceException(value);
            if (_buyerPrice > 0 && value < _buyerPrice)
                throw new InvalidPriceException(value, _buyerPrice, Name ?? "Product");

            _salePrice = value;
        }
    }

    private decimal _wholesalePrice;

    
    public decimal WholesalePrice
    {
        get => _wholesalePrice;
        set
        {
            if (value < 0 || value == 0)
                throw new InvalidPriceException(value);

            _wholesalePrice = value;
        }
    }

    public int CurrentStock { get; set; }
    public int MinimumStock { get; set; }

    public string? Mark { get; set; }
     public string? Model { get; set; }
    public string? Color { get; set; }
     public decimal? Weight { get; set; } // in Kg
     public string? Size { get; set; } // E.g: 2m x 1m

    //Control
    public bool RequiredRefrigeration { get; set; } = false;
    public bool DangerousMaterial { get; set; } = false;
    public bool Active { get; set; } = true;
    public DateTime DateCreated { get; private set; } = DateTime.UtcNow;
    public DateTime? DateUpdated { get; set; }

    public ICollection<SalesDetail> SalesDetails { get; private set; } = new List<SalesDetail>();

    public ICollection<InventoryMovement> InventoryMovements { get; private set; } =
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

