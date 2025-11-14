
using Domain.Enums;


namespace Domain.Entities;

public  class Customer
{
    
    public int Id { get; set; }
    public TypeCustomer TypeCustomer { get; set; } = TypeCustomer.Retail;
    
    public string FullName { get; set; } = string.Empty;
    
    public string? Nit { get; set; }
    
    public string DocumentNumber { get; set; } = string.Empty;
    
    public string? PhoneNumber { get; set; }
    
    public string? Email { get; set; }
    
    public string? City { get; set; }
    
    public string? Country { get; set; }
    //Business Information
    
    public decimal CreditLimit { get; set; } = 0;
    
    public int DaysToPay { get; set; } = 0;
    
    public decimal SpecialDiscount { get; set; }
    public bool Active { get; set; } = true;
    public DateTime DateRegistered { get; set; } = DateTime.UtcNow;
    public ICollection<Sale> Sales { get; set; } = new List<Sale>();
}
