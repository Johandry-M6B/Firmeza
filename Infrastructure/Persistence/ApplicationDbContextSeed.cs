using Domain.Entities;

namespace Infrastructure.Persistence;

public class ApplicationDbContextSeed
{
    public static async Task SeedAsync(ApplicationDbContext context)
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

        await context.SaveChangesAsync();
    }
}