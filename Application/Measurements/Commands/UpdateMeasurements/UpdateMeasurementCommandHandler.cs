using Domain.Exceptions;
using Domain.Interfaces;
using MediatR;

namespace Application.Measurements.Commands.UpdateMeasurements;

public class UpdateMeasurementCommandHandler : IRequestHandler<UpdateMeasurementCommand, Unit>
{
    private readonly IMeasurementRepository _measurementRepository;

    public UpdateMeasurementCommandHandler(IMeasurementRepository measurementRepository)
    {
        _measurementRepository = measurementRepository;
    }

    public async Task<Unit> Handle(UpdateMeasurementCommand request, CancellationToken cancellationToken)
    {
        var measurement = await _measurementRepository.GetByIdAsync(request.Id);

        if (measurement == null)
        {
            throw new EntityNotFoundException(nameof(Measurements), request.Id);
        }

        measurement.Name = request.Name;
        measurement.Abbreviation = request.Abbreviation;

        await _measurementRepository.UpdateAsync(measurement);

        return Unit.Value;
    }
}