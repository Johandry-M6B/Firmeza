
namespace Domain.Entities;

public  class Measurement
{
   
    public int Id { get; set; }
    
    public string Name { get; set; } = null!;
   
    public string Abbreviation { get; set; } = null!;
    public bool Active { get; set; } = true;
    public  ICollection<Product> Products { get; set; } = new List<Product>();
}