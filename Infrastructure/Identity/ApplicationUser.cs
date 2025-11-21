using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity;

public class ApplicationUser: IdentityUser
{
    [Required]
    [MaxLength(100)]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string LastName { get; set; } = string.Empty;

    [MaxLength(20)]
    public string? DocumentNumber { get; set; }

    [MaxLength(200)]
    public string? Address { get; set; }

    [MaxLength(100)]
    public string? City { get; set; }

    public DateTime DateRegistered { get; set; } = DateTime.UtcNow;

    public bool IsActive { get; set; } = true;

    // Propiedad calculada para nombre completo
    public string FullName => $"{FirstName} {LastName}";
}
public class ApplicationRole : IdentityRole
{
    [MaxLength(200)]
    public string? Description { get; set; }
}