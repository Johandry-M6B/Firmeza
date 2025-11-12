using Domain.Entities;

namespace Domain.Interfaces;

public interface IProductRepository : IRepository<Product>
{
    Task<Product?> GetByCodeAsync(string code);
    Task<IEnumerable<Product>> GetByCategoryAsync(int categoryId);
    Task<IEnumerable<Product>> GetLowStockProductsAsync();
    Task<IEnumerable<Product>> GetActiveProductAsync();
    Task<bool> ExistsCodeAsync(string code);
    Task<IEnumerable<Product>> SearchAsync(string searchTerm);
}