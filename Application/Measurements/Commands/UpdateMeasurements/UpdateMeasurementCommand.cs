using MediatR;

namespace Application.Measurements.Commands.UpdateMeasurements;

public class UpdateMeasurementCommand : IRequest<Unit>
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Abbreviation { get; set; } = string.Empty;
}