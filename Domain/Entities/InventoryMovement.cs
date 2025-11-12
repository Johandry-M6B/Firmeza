
using Domain.Enums;


namespace Domain.Entities;

public  class InventoryMovement
{
    
    public int Id { get; set; }
    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;
    public MovementType MovementType { get; set; }
    public DateTime Date { get; set; } = DateTime.UtcNow;
   
    public int Quantity { get; set; }
    public int AfterStock { get; set; }
    public int NewStock { get; set; }
 
    public string? Observation { get; set; }
    

    
    public DateTime DateCreated  { get; set; } = DateTime.UtcNow;

    
}
