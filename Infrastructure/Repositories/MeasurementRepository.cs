using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class MeasurementRepository : Repository<Measurement>, IMeasurementRepository
{
    public MeasurementRepository(ApplicationDbContext context) : base(context)
    {
        
    }

    public async Task<IEnumerable<Measurement>> GetActiveAsync()
    {
        return await _dbSet
            .Where(m => m.Active)
            .OrderBy(m => m.Name)
            .ToListAsync();
    }

    public async Task<bool> HasProductsAsync(int measurementId)
    {
        return await _context.Products
            .AnyAsync(p => p.MeasurementId == measurementId);
    }   
    
}