using System.ComponentModel.DataAnnotations;
using Domain.Entities;

namespace Firmeza.Web.Data.Entities;

public class Supplier
{
    [Key]
    public int Id { get; set; }
    [Required]
    [MaxLength(20)]
    public string TradeName { get; set; } 
    [Required]
    [MaxLength(20)]
    public string Nit { get; set; }
    [MaxLength(100)]
    public string ContactName { get; set; }
    [MaxLength(15)]
    public string PhoneNumber { get; set; }
    [MaxLength(100)]
    [EmailAddress]
    public string Email { get; set; }
    [MaxLength(200)]
    public string Address { get; set; }
    [MaxLength(100)]
    public string City { get; set; }
    public bool Active { get; set; } = true;
    public DateTime DateCreated { get; set; } = DateTime.UtcNow;
    public virtual ICollection<Product> Products { get; set; } = [];
}