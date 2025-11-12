using Domain.Entities;
using Domain.Enums;

namespace Domain.Interfaces;

public interface IInventoryMovementRepository : IRepository<InventoryMovement>
{
    Task<IEnumerable<InventoryMovement>> GetByProductAsync(int productId);
    Task<IEnumerable<InventoryMovement>> GetByTypeAsync(MovementType type);
    Task<IEnumerable<InventoryMovement>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
}