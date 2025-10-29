using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Firmeza.Web.Data.Entities;

public enum PaymentFrom
{
    Cash = 1,
    Transfer = 2,
    CreditCard = 3,
    DebitCard = 4,
    Check = 5,
    Credit = 6
}

public enum SaleStatus
{
    Pending = 1,
    Paid = 2,
    Cancelled = 3,
    Credit = 4,
    PartiallyPaid = 5
}

public class Sale
{
    [Key]
    public int Id { get; set; }
    [Required]
    [MaxLength(20)]
    public string InvoiceNumber { get; set; } = string.Empty;
    public DateTime Date { get; set; } = DateTime.UtcNow;
    
    //Customer
    [Required]
    public int CustomerId { get; set; }
    [ForeignKey(nameof(CustomerId))] 
    public virtual Customer Customer { get; set; } = null!;
    //Totals
    [Column(TypeName = "decimal(18,2)")]
    public decimal SubTotal { get; set; }

    [Column(TypeName = "decimal(18,2)")] 
    public decimal Discount { get; set; } = 0;
    [Column(TypeName = "decimal(18,2)")]   
    public decimal Vat { get; set; }
    [Column(TypeName = "decimal(18,2)")]
    public decimal Total { get; set; }
    //Payment
    public PaymentFrom PaymentFrom { get; set; }
    public SaleStatus Status { get; set; } = SaleStatus.Pending;
    [Column(TypeName = "decimal(18,2)")]
    public decimal AmountPaid { get; set; } = 0;
    [Column(TypeName = "decimal(18,2)")]
    public decimal Balance { get; set; } = 0;
    public DateTime? FullPaymentDate { get; set; }
    //Delivery
    [MaxLength(200)]
    public string? DeliveryAddress { get; set; }
    public DateTime? DeliveryDate { get; set; }
    public bool Devoted { get; set; } = false;
    //Observations
    [MaxLength(500)]
    public string? Observations { get; set; }
    public DateTime DateCreated { get; set; } = DateTime.UtcNow;
    
    public virtual ICollection<SalesDetail> SalesDetails { get; set; } = new List<SalesDetail>();
    public virtual ICollection<PaymentSale> PaymentSales { get; set; } = new List<PaymentSale>();
}