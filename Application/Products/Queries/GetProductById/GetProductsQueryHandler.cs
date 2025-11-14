using Application.Products.DTOs;
using AutoMapper;
using Domain.Interfaces;
using MediatR;

namespace Application.Products.Queries.GetProductById;

public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, IEnumerable<ProductDto>>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public GetProductsQueryHandler(
        IProductRepository productRepository,
        IMapper mapper)
    {
        _productRepository = productRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ProductDto>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<Domain.Entities.Product> products;

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            products = await _productRepository.SearchAsync(request.SearchTerm);
        }
        else if (request.CategoryId.HasValue)
        {
            products = await _productRepository.GetByCategoryAsync(request.CategoryId.Value);
        }
        else if (request.OnlyActive)
        {
            products = await _productRepository.GetActiveProductAsync();
        }
        else
        {
            products = await _productRepository.GetAllAsync();
        }
        
        return _mapper.Map<IEnumerable<ProductDto>>(products);
    }
    
}