using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Enums;
using Firmeza.Web.Data.Entities;

namespace Domain.Entities;

public class Customer
{
    [Key]
    public int Id { get; set; }
    public TypeCustomer TypeCustomer { get; set; } = TypeCustomer.Retail;
    [Required]
    [MaxLength(200)]
    public string FullName { get; set; } = string.Empty;
    [MaxLength(20)]
    public string? Nit { get; set; }
    [Required]
    [MaxLength(15)]
    public string DocumentNumber { get; set; } = string.Empty;
    [MaxLength(15)]
    public string? PhoneNumber { get; set; }
    [MaxLength(100)]
    [EmailAddress]
    public string? Email { get; set; }
    [MaxLength(100)]
    public string? City { get; set; }
    [MaxLength(100)]
    public string? Country { get; set; }
    //Business Information
    [Column(TypeName = "decimal(18,2)")]
    public decimal CreditLimit { get; set; } = 0;
    
    public int DaysToPay { get; set; } = 0;
    
    [Column(TypeName = "decimal(18,2)")]
    public decimal SpecialDiscount { get; set; }
    public bool Active { get; set; } = true;
    public DateTime DateRegistered { get; set; } = DateTime.UtcNow;
    public virtual ICollection<Sale> Sales { get; set; } = new List<Sale>();
}
