using Domain.Entities;

namespace Domain.Interfaces;

public interface ICategoryRepository : IRepository<Category>
{
    Task<IEnumerable<Category>> GetActiveAsync();
    Task<bool> HasProductsAsync(int categoryId);
    
}