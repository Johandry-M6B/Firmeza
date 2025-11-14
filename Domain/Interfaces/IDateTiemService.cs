using System.Runtime.InteropServices.JavaScript;

namespace Domain.Interfaces;

public interface IDateTimeService
{
    DateTime Now { get; }
    DateTime UtcNow { get; }
}