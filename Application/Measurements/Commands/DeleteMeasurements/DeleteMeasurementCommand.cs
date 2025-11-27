using MediatR;

namespace Application.Measurements.Commands.DeleteMeasurements;

public class DeleteMeasurementCommand : IRequest<Unit>
{
    public int Id { get; set; }
}