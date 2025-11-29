using Domain.Entities;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Persistence;

public class ApplicationDbContextSeed
{
    public static async Task SeedAsync(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        // Seed Categories
        if (!context.Categories.Any())
        {
            context.Categories.AddRange(
                new Category 
                { 
                    Name = "Electrónica", 
                    Description = "Productos electrónicos", 
                    Active = true,
                    DateCreated = DateTime.UtcNow
                },
                new Category 
                { 
                    Name = "Alimentos", 
                    Description = "Productos alimenticios", 
                    Active = true,
                    DateCreated = DateTime.UtcNow
                },
                new Category 
                { 
                    Name = "Bebidas", 
                    Description = "Bebidas variadas", 
                    Active = true,
                    DateCreated = DateTime.UtcNow
                },
                new Category 
                { 
                    Name = "Limpieza", 
                    Description = "Productos de limpieza", 
                    Active = true,
                    DateCreated = DateTime.UtcNow
                },
                new Category 
                { 
                    Name = "Ferretería", 
                    Description = "Herramientas y materiales", 
                    Active = true,
                    DateCreated = DateTime.UtcNow
                }
            );
        }

        // Seed Measurements
        if (!context.Measurements.Any())
        {
            context.Measurements.AddRange(
                new Measurement 
                { 
                    Name = "Unidad", 
                    Abbreviation = "Und", 
                    Active = true 
                },
                new Measurement 
                { 
                    Name = "Kilogramo", 
                    Abbreviation = "Kg", 
                    Active = true 
                },
                new Measurement 
                { 
                    Name = "Litro", 
                    Abbreviation = "Lt", 
                    Active = true 
                },
                new Measurement 
                { 
                    Name = "Metro", 
                    Abbreviation = "Mt", 
                    Active = true 
                },
                new Measurement 
                { 
                    Name = "Caja", 
                    Abbreviation = "Cja", 
                    Active = true 
                },
                new Measurement 
                { 
                    Name = "Paquete", 
                    Abbreviation = "Pqt", 
                    Active = true 
                }
            );
        }

        // Seed Default User
        var defaultUser = new ApplicationUser 
        { 
            UserName = "admin@firmeza.com", 
            Email = "admin@firmeza.com",
            FirstName = "Admin",
            LastName = "Firmeza",
            IsActive = true
        };

        if (!context.Users.Any(u => u.UserName == defaultUser.UserName))
        {
            var result = await userManager.CreateAsync(defaultUser, "Admin123*");
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new Exception($"Failed to create default user: {errors}");
            }
        }

        await context.SaveChangesAsync();
    }
}