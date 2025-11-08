using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Enums;
using Firmeza.Web.Data.Entities;

namespace Domain.Entities;

public class InventoryMovement
{
    [Key]
    public int Id { get; set; }
    public int ProductId { get; set; }
    [ForeignKey(nameof(ProductId))]
    public virtual Product Product { get; set; } = null!;
    public MovementType MovementType { get; set; }
    public DateTime Date { get; set; } = DateTime.UtcNow;
    [Column(TypeName = "decimal(18,2)")]
    public int Quantity { get; set; }
    public int AfterStock { get; set; }
    public int NewStock { get; set; }
    [MaxLength(200)]
    public string? Observation { get; set; }
    

    
    public DateTime DateCreated  { get; set; } = DateTime.UtcNow;

    
}
