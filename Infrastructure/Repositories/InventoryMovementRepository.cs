using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class InventoryMovementRepository : Repository<InventoryMovement>, IInventoryMovementRepository
{
    public InventoryMovementRepository(ApplicationDbContext context) : base(context)
    {
    }
    public async Task<IEnumerable<InventoryMovement>> GetByProductAsync(int productId)
    {
        return await _dbSet
            .Include(im => im.Product)
            .Where(im => im.ProductId == productId)
            .OrderByDescending(im => im.Date)
            .ToListAsync();
    }

    public async Task<IEnumerable<InventoryMovement>> GetByTypeAsync(MovementType type)
    {
        return await _dbSet
            .Include(im => im.Product)
            .Where(im => im.MovementType == type)
            .OrderByDescending(im => im.Date)
            .ToListAsync();
    }

    public async Task<IEnumerable<InventoryMovement>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        return await _dbSet
            .Include(im => im.Product)
            .Where(im => im.Date >= startDate && im.Date <= endDate)
            .OrderByDescending(im => im.Date)
            .ToListAsync();
    }
}