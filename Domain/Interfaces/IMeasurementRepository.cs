using Domain.Entities;

namespace Domain.Interfaces;

public interface IMeasurementRepository : IRepository<Measurement>
{
    Task<IEnumerable<Measurement>> GetActiveAsync();
    Task<bool> HasProductsAsync(int measurementId);
}