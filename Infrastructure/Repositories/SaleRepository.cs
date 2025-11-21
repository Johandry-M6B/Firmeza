using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Repositories;

public class SaleRepository : Repository<Sale>, ISaleRepository
{
    public SaleRepository(ApplicationDbContext context) : base(context)
    {
        
    }
    public override async Task<Sale?> GetByIdAsync(int id)
    {
        return await _dbSet
            .Include(s => s.Customer)
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    

    public async Task<Sale?> GetByIdWithDetailsAsync(int id)
    {
        return await _dbSet
            .Include(s => s.Customer)
            .Include(s => s.SalesDetails)
            .ThenInclude(sd => sd.Product)
            .Include(s => s.PaymentSales)
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<IEnumerable<Sale>> GetByCustomerAsync(int customerId)
    {
        return await _dbSet
            .Include(s => s.Customer)
            .Where(s => s.CustomerId == customerId)
            .OrderByDescending(s => s.Date)
            .ToListAsync();
    }

    public async Task<IEnumerable<Sale>> GetByStatusAsync(SaleStatus status)
    {
        return await _dbSet
            .Include(s => s.Customer)
            .Where(s => s.Status == status)
            .OrderByDescending(s => s.Date)
            .ToListAsync();
    }

    public async Task<IEnumerable<Sale>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        return await _dbSet
            .Include(s => s.Customer)
            .Where(s => s.Date >= startDate && s.Date <= endDate)
            .OrderByDescending(s => s.Date)
            .ToListAsync();
    }

    public async Task<decimal> GetTotalSalesByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        return await _dbSet
            .Where(s => s.Date >= startDate && s.Date <= endDate && s.Status == SaleStatus.Paid)
            .SumAsync(s => s.Total);
    }

    public async Task<IEnumerable<Sale>> GetPendingSaleAsync()
    {
        return await _dbSet
            .Include(s => s.Customer)
            .Where(s => s.Status == SaleStatus.Pending)
            .OrderBy(s => s.Date)
            .ToListAsync();
    }
    
    
}