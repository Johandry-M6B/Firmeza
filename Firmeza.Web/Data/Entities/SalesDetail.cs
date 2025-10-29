using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Firmeza.Web.Data.Entities;

public class SalesDetail
{
    [Key]
    public int Id { get; set; }
    public int SaleId { get; set; }
    [ForeignKey(nameof(SaleId))]
    public virtual Sale Sale { get; set; }
    public int ProductId { get; set; }
    [ForeignKey(nameof(ProductId))]
    public virtual Product Product { get; set; } = null!;
    [Column(TypeName = "decimal(18,2)")]
    public int Quantity { get; set; }
    [Column(TypeName = "decimal(18,2)")]
    public decimal Price { get; set; }

    [Column(TypeName = "decimal(18,2)")] 
    public decimal Discount { get; set; } = 0;

    [Column(TypeName = "decimal(18,2)")] 
    public decimal SubTotal { get; set; }

    [Column(TypeName = "decimal(18,2)")]    
    public decimal VatPercentage { get; set; } = 19;
    [Column(TypeName = "decimal(18,2)")]
    public decimal Vat { get; set;}
    [Column(TypeName  = "decimal(18,2)")]
    public decimal Total { get; set; }

    [MaxLength(200)] 
    public string? Observations { get; set; }
}
