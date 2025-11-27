// Firmeza.Application/Categories/Commands/CreateCategory/CreateCategoryCommandHandler.cs

using Domain.Entities;
using Domain.Interfaces;
using MediatR;

namespace Application.Categories.Commands.CreateCategory;

public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, int>
{
    private readonly ICategoryRepository _categoryRepository;

    public CreateCategoryCommandHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<int> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = new Category
        {
            Name = request.Name,
            Description = request.Description,
            Active = true,
            DateCreated = DateTime.UtcNow
        };

        var createdCategory = await _categoryRepository.AddAsync(category);
        return createdCategory.Id;
    }
}