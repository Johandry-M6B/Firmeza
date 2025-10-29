using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Firmeza.Web.Data.Entities;
public enum MovementType
{
    Input = 1, // Purchase from supplier
    Output = 2, // Sale
    Adjustment = 3, // Manual adjustment
    Return = 4, // Return from customer
    Decrease = 5, // Decrease due to damage or loss
    Transfer = 6 // Transfer between locations
}
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
