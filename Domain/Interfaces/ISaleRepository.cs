using Domain.Entities;
using Domain.Enums;

namespace Domain.Interfaces;

public interface ISaleRepository : IRepository<Sale>
{
    Task<Sale?> GeByidWithDetailsAsync(int id);
    Task<IEnumerable<Sale>> GetByCustomerAsync(int customerId);
    Task<IEnumerable<Sale>> GetByStatusAsync(SaleStatus status);
    Task<IEnumerable<Sale>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
    Task<decimal> GetTotalSalesByDateRangeAsync(DateTime startDate, DateTime endDate);
    Task<IEnumerable<Sale>> GetPendingSaleAsync();
}