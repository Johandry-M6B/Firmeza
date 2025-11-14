using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Domain.Entities;

public  class Category
{
    
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public bool Active { get; set; } = true;
    public DateTime DateCreated { get; set; } = DateTime.UtcNow;
    public ICollection<Product> Products { get; set; } = new List<Product>();

}