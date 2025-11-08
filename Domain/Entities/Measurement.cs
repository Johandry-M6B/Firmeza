using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Domain.Entities;

public class Measurement
{
    [Key]
    public int Id { get; set; }
    [Required]
    [MaxLength(50)]
    public string Name { get; set; } = null!;
    [Required]
    [MaxLength(10)]
    public string Abbreviation { get; set; } = null!;
    public bool Active { get; set; } = true;
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}