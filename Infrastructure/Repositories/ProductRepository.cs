using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using ApplicationDbContext = Infrastructure.Persistence.ApplicationDbContext;

namespace Infrastructure.Repositories;


public class ProductRepository : Repository<Product>, IProductRepository
{
    public ProductRepository(ApplicationDbContext context) : base(context)
    {
    }

    public override async Task<Product?> GetByIdAsync(int id)
    {
        return await _dbSet
            .Include(p => p.Category)
            .Include(p => p.Measurement)
            .Include(p => p.Supplier)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public override async Task<IEnumerable<Product>> GetAllAsync()
    {
        return await _dbSet
            .Include(p => p.Category)
            .Include(p => p.Measurement)
            .Include(p => p.Supplier)
            .OrderBy(p => p.Name)
            .ToListAsync();
    }

    public async Task<Product?> GetByCodeAsync(string code)
    {
        return await _dbSet
            .Include(p => p.Category)
            .Include(p => p.Measurement)
            .Include(p => p.Supplier)
            .FirstOrDefaultAsync(p => p.Code == code);
    }

    public async Task<IEnumerable<Product>>GetByCategoryAsync(int categoryId)
    {
        return await _dbSet
            .Include(p => p.Category)
            .Include(p => p.Measurement)
            .Include(p => p.Supplier)
            .Where(p => p.CategoryId == categoryId)
            .OrderBy(p => p.Name)
            .ToListAsync();
    }

    public async Task<IEnumerable<Product>> GetLowStockProductsAsync()
    {
        return await _dbSet
            .Include(p => p.Category)
            .Include(p => p.Measurement)
            .Where(p => p.CurrentStock <= p.MinimumStock && p.Active)
            .OrderBy(p => p.Name)
            .ToListAsync();
        }

    public  async Task<IEnumerable<Product>> GetActiveProductAsync()
    {
        return await _dbSet
            .Include(p => p.Category)
            .Include(p => p.Measurement)
            .Include(p => p.Supplier)
            .Where(p => p.Active)
            .OrderBy(p => p.Name)
            .ToListAsync();
    }

   

    public async Task<bool> ExistsCodeAsync(string code)
    {
        return await _dbSet.AnyAsync(p => p.Code == code);
    }

    public async Task<IEnumerable<Product>> SearchAsync(string searchTerm)
    {
        var term = searchTerm.ToLower();
        
        return await _dbSet
            .Include(p => p.Category)
            .Include(p => p.Measurement)
            .Include(p => p.Supplier)
            .Where(p => p.Name.ToLower().Contains(term) || 
                        p.Code.ToLower().Contains(term) || 
                        (p.Description != null && p.Description
                            .ToLower()
                            .Contains(term)))
            .OrderBy(p => p.Name)
            .ToListAsync();
    }
}