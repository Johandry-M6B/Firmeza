using System.Runtime.InteropServices.JavaScript;

namespace Domain.Interfaces;

public interface IDateTiemService
{
    DateTime Now { get; }
    DateTime UtcNow { get; }
}