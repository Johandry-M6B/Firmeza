
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Repositories;

public class CustomerRepository : Repository<Customer>, ICustomerRepository
{
    public CustomerRepository(ApplicationDbContext context) : base(context)
    {
        
    }
    public async Task<Customer?> GetByDocumentAsync(string documentNumber)
    {
        return await _dbSet
            .FirstOrDefaultAsync(c => c.DocumentNumber == documentNumber);
    }

    public async Task<Customer?> GetByEmailAsync(string email)
    {
        return await _dbSet
            .FirstOrDefaultAsync(c => c.Email == email);
    }
    public async Task<IEnumerable<Customer>> GetByTypeAsync(TypeCustomer type)
    {
        return await _dbSet
            .Where(c => c.TypeCustomer == type)
            .OrderBy(c => c.FullName)
            .ToListAsync();
    }

    public async Task<IEnumerable<Customer>> GetActiveAsync()
    {
        return await _dbSet
            .Where(c => c.Active)
            .OrderBy(c => c.FullName)
            .ToListAsync();
    }

    public async Task<bool> ExistsDocumentAsync(string documentNumber)
    {
        return await _dbSet
            .AnyAsync(c => c.DocumentNumber == documentNumber);
    }

    public async Task<IEnumerable<Customer>> SearchAsync(string searchTerm)
    {
        var term = searchTerm.ToLower();
        
        return await _dbSet
            .Where(c => c.FullName.ToLower().Contains(term) ||
                        c.DocumentNumber.Contains(term) ||
                        (c.Email != null && c.Email.ToLower().Contains(term)) ||
                        (c.PhoneNumber != null && c.PhoneNumber.Contains(term)))
            .OrderBy(c => c.FullName)
            .ToListAsync();
    }
}