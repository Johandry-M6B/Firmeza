using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public  class SalesDetail
{
    
    public int Id { get; set; }
    public int SaleId { get; set; }
    public  Sale Sale { get; set; }
    public int ProductId { get; set; }
    public  Product Product { get; set; } = null!;
    
    public int Quantity { get; set; }
   
    public decimal Price { get; set; }

   
    public decimal Discount { get; set; } = 0;

    
    public decimal SubTotal { get; set; }

       
    public decimal VatPercentage { get; set; } = 19;
    
    public decimal Vat { get; set;}
    
    public decimal Total { get; set; }

    [MaxLength(200)] 
    public string? Observations { get; set; }
}
