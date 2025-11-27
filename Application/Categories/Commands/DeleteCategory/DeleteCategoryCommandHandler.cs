using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces;
using MediatR;

namespace Application.Categories.Commands.DeleteCategory;

public class DeleteCategoryCommandHandler :IRequestHandler<DeleteCategoryCommand, Unit>
{
    private readonly ICategoryRepository _categoryRepository;

    public DeleteCategoryCommandHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<Unit> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetByIdAsync(request.Id);

        if (category == null)
        {
            throw new EntityNotFoundException(nameof(Category), request.Id);
        }

        var hasProducts = await _categoryRepository.HasProductsAsync(request.Id);
        if (hasProducts)
        {
            throw new InvalidOperationException("Cannot delete category with associated products.");
        }

        await _categoryRepository.DeleteAsync(request.Id);

        return Unit.Value;
    }
}