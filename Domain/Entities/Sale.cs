
using Domain.Enums;

namespace Domain.Entities;

public  class Sale
{
   
    public int Id { get; set; }
   
    public string InvoiceNumber { get; set; } = string.Empty;
    public DateTime Date { get; set; } = DateTime.UtcNow;
    
    //Customer
  
    public int CustomerId { get; set; }
    public Customer Customer { get; set; } = null!;
    //Totals
    
    public decimal SubTotal { get; set; }

    
    public decimal Discount { get; set; } = 0;
      
    public decimal Vat { get; set; }
   
    public decimal Total { get; set; }
    //Payment
    public PaymentFrom PaymentFrom { get; set; }
    public SaleStatus Status { get; set; } = SaleStatus.Pending;
    
    public decimal AmountPaid { get; set; } = 0;
   
    public decimal Balance { get; set; } = 0;
    public DateTime? FullPaymentDate { get; set; }
    //Delivery
    
    public string? DeliveryAddress { get; set; }
    public DateTime? DeliveryDate { get; set; }
    public bool Devoted { get; set; } = false;
    //Observations
    
    public string? Observations { get; set; }
    public DateTime DateCreated { get; set; } = DateTime.UtcNow;
    
    public virtual ICollection<SalesDetail> SalesDetails { get; set; } = new List<SalesDetail>();
    public virtual ICollection<PaymentSale> PaymentSales { get; set; } = new List<PaymentSale>();
}