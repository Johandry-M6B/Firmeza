using Domain.Entities;
using Domain.Enums;

namespace Domain.Interfaces;

public interface ICustomerRepository : IRepository<Customer>
{
    Task<Customer?> GetByDocumentAsync(string documentNumber);
    Task<Customer?> GetByEmailAsync(string email);
    Task<IEnumerable<Customer>> GetByTypeAsync(TypeCustomer type);
    Task<IEnumerable<Customer>> GetActiveAsync();
    Task<bool> ExistsDocumentAsync(string documentNumber);
    Task<IEnumerable<Customer>> SearchAsync(string searchTerm);
}