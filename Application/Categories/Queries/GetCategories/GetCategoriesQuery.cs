// Firmeza.Application/Categories/Queries/GetCategories/GetCategoriesQuery.cs

using Application.Categories.DTOs;
using MediatR;

namespace Application.Categories.Queries.GetCategories;

public class GetCategoriesQuery : IRequest<IEnumerable<CategoryDto>>
{
    public bool OnlyActive { get; set; } = true;
}