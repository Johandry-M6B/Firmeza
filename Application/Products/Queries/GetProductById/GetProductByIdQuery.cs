using Application.Products.DTOs;
using Domain.Interfaces;
using MediatR;

namespace Application.Products.Queries.GetProductById;

public class GetProductByIdQuery : IRequest<ProductDto?>
{
    private IRepository<ProductDto?> _repositoryImplementation;
    public int Id { get; set; }

    public GetProductByIdQuery(int id)
    {
        Id = id;
    }
}

