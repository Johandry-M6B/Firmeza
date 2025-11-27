// Firmeza.Application/Categories/Queries/GetCategories/GetCategoriesQueryHandler.cs

using Application.Categories.DTOs;
using AutoMapper;
using Domain.Interfaces;
using MediatR;

namespace Application.Categories.Queries.GetCategories;

public class GetCategoriesQueryHandler : IRequestHandler<GetCategoriesQuery, IEnumerable<CategoryDto>>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IMapper _mapper;

    public GetCategoriesQueryHandler(
        ICategoryRepository categoryRepository,
        IMapper mapper)
    {
        _categoryRepository = categoryRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<CategoryDto>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
    {
        var categories = request.OnlyActive
            ? await _categoryRepository.GetActiveAsync()
            : await _categoryRepository.GetAllAsync();

        return _mapper.Map<IEnumerable<CategoryDto>>(categories);
    }
}