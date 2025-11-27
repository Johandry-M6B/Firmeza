namespace Domain.Entities;

public  class Supplier
{
   
    public int Id { get; set; }
   
    public string TradeName { get; set; } 
    
    public string Nit { get; set; }
   
    public string ContactName { get; set; }
   
    public string PhoneNumber { get; set; }
   
    public string Email { get; set; }
   
    public string Address { get; set; }
   
    public string City { get; set; }
    public bool Active { get; set; } = true;
    public DateTime DateCreated { get; set; } = DateTime.UtcNow;
    public ICollection<Product> Products { get; set; } = [];
}