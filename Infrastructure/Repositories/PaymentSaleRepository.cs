using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class PaymentSaleRepository : Repository<PaymentSale>, IPaymentSaleRepository
{
    public PaymentSaleRepository(ApplicationDbContext context) : base(context)
    {
        
    }
    public async Task<IEnumerable<PaymentSale>> GetBySaleAsync(int saleId)
    {
        return await _dbSet
            .Where(p => p.SaleId == saleId)
            .OrderBy(p => p.PaymentDate)
            .ToListAsync();
    }

    public async Task<decimal> GetTotalPaidBySaleAsync(int saleId)
    {
        return await _dbSet
            .Where(p => p.SaleId == saleId)
            .SumAsync(p => p.Amount);
    }
    
}