using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class CategoryRepository: Repository<Category>,ICategoryRepository
{
    public CategoryRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Category>> GetActiveAsync()
    {
        return await _dbSet
            .Where(c => c.Active)
            .OrderBy(c => c.Name)
            .ToListAsync();
    }

    public async Task<bool> HasProductsAsync(int categoryId)
    {
        return await _context.Products
            .AnyAsync(p => p.CategoryId == categoryId);
    }
    
}