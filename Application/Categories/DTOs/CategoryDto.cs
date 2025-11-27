// Firmeza.Application/Categories/DTOs/CategoryDto.cs
namespace Application.Categories.DTOs;

public class CategoryDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool Active { get; set; }
    public DateTime DateCreated { get; set; }
}