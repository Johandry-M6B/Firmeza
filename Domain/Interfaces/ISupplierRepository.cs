using Domain.Entities;

namespace Domain.Interfaces;

public interface ISupplierRepository : IRepository<Supplier>
{
    Task<IEnumerable<Supplier>> GetActiveAsync();
    Task<bool> HasProductsAsync(int supplierId);
    Task<IEnumerable<Supplier>> SearchAsync(string searchTerm);
}