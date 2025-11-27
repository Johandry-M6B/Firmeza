using Domain.Entities;

namespace Domain.Interfaces;

public interface IPaymentSaleRepository : IRepository<PaymentSale>
{
    Task<IEnumerable<PaymentSale>> GetBySaleAsync(int saleId);
    Task<decimal> GetTotalPaidBySaleAsync(int saleId);
}