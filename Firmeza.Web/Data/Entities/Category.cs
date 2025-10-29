using System.ComponentModel.DataAnnotations;

namespace Firmeza.Web.Data.Entities;

public class Category
{
    [Key]
    public int Id { get; set; }
    [Required]
    [MaxLength(100)]
    public string Name { get; set; }
    [MaxLength(500)]    
    public string Description { get; set; }

    public bool Active { get; set; } = true;
    public DateTime DateCreated { get; set; } = DateTime.UtcNow;
    public virtual ICollection<Product> Products { get; set; } = [];

}