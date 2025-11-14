using Application.Products.DTOs;
using MediatR;

namespace Application.Products.Queries.GetProductById;

public class GetProductsQuery : IRequest<IEnumerable<ProductDto>>
{
    public bool OnlyActive { get; set; } = true;
    public string? SearchTerm { get; set; }
    public int? CategoryId { get; set; }    

}