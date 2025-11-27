using MediatR;

namespace Application.Measurements.Queries.CreateMeasurement;

public class CreateMeasurementCommand : IRequest<int>
{
    public string Name { get; set; } = string.Empty;
    public string Abbreviation { get; set; } = string.Empty;
    
}