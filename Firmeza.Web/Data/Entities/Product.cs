using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace Firmeza.Web.Data.Entities;

public class Product
{
    [Key]
    public int Id { get; set; }
    [Required]
    [MaxLength(20)]
    public string Code { get; set; } 
    [Required]
    [MaxLength(100)]
    public string Name { get; set; }
    [MaxLength(500)]
    public string Description { get; set; }
    [Required]
    public int CategoryId { get; set; }
    [ForeignKey(nameof(CategoryId))]
    public virtual Category Category { get; set; }
    [Required]
    public int MeasurementId { get; set; }
    [ForeignKey(nameof(MeasurementId))]
    public virtual Measurement Measurement { get; set; }
    
    public int? SupplierId { get; set; }
    [ForeignKey(nameof(SupplierId))]
    public virtual Supplier Supplier { get; set; }
    
    //Price e Inventory
    [Column(TypeName = "decimal(18,2)")]
    public decimal BuyerPrice { get; set; }
    [Column(TypeName = "decimal(18,2)")]
    public decimal SalePrice { get; set; }
    [Column(TypeName = "decimal(18,2)")]
    public decimal WholesalePrice { get; set; }
    
    public int CurrentStock { get; set; }
    public int MinimumStock { get; set; }
    
    [MaxLength(50)]
    public string? Mark { get; set; }
    [MaxLength(100)]
    public string? Model { get; set; }
    [MaxLength(50)]
    public string? Color { get; set; }
    [Column(TypeName = "decimal(18,2)")]
    public decimal? Weight { get; set; }// in Kg
    [MaxLength(50)]
    public string? Size { get; set; } // E.g: 2m x 1m
    
    //Control
    public bool RequiredRefrigeration { get; set; } = false;
    public bool DangerousMaterial { get; set; } = false;
    public bool Active { get; set; } = true;
    public DateTime DateCreated { get; set; } = DateTime.UtcNow;
    public DateTime? DateUpdated { get; set; }
    
    public virtual ICollection<SalesDetail> SalesDetails { get; set; } = new List<SalesDetail>();
    public virtual ICollection<InventoryMovement> InventoryMovements { get; set; } = new List<InventoryMovement>();
}