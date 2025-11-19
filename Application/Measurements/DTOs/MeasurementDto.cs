// Firmeza.Application/Measurements/DTOs/MeasurementDto.cs
namespace Firmeza.Application.Measurements.DTOs;

public class MeasurementDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Abbreviation { get; set; } = string.Empty;
    public bool Active { get; set; }
}