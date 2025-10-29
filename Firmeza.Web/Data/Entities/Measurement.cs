using System.ComponentModel.DataAnnotations;

namespace Firmeza.Web.Data.Entities;

public class Measurement
{
    [Key]
    public int Id { get; set; }
    [Required]
    [MaxLength(50)]
    public string Name { get; set; }
    [Required]
    [MaxLength(10)]
    public string Abbreviation { get; set; }
    public bool Active { get; set; } = true;
    public virtual ICollection<Product> Products { get; set; } = [];
}