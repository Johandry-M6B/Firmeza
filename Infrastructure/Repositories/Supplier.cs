using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Repositories;

public class SupplierRepository : Repository<Supplier>, ISupplierRepository
{
    public SupplierRepository(ApplicationDbContext context) : base(context)
    {
        
    }
    
    public async Task<IEnumerable<Supplier>> GetActiveAsync()
    {
        return await _dbSet
            .Where(s => s.Active)
            .OrderBy(s => s.TradeName)
            .ToListAsync();
    }

    public async Task<bool> HasProductsAsync(int supplierId)
    {
        return await _context.Products
            .AnyAsync(p => p.SupplierId == supplierId);
    }

    public async Task<IEnumerable<Supplier>> SearchAsync(string searchTerm)
    {
        var term = searchTerm.ToLower();
        
        return await _dbSet
            .Where(s => s.TradeName.ToLower().Contains(term) ||
                        s.Nit.Contains(term) ||
                        (s.ContactName != null && s.ContactName
                            .ToLower()
                            .Contains(term)))
            .OrderBy(s => s.TradeName)
            .ToListAsync();
    }
}