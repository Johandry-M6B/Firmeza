using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces;
using MediatR;

namespace Application.Measurements.Commands.DeleteMeasurements;

public class DeleteMeasurementCommandHandler : IRequestHandler<DeleteMeasurementCommand, Unit>
{
    private readonly IMeasurementRepository _measurementRepository;

    public DeleteMeasurementCommandHandler(IMeasurementRepository measurementRepository)
    {
        _measurementRepository = measurementRepository;
    }

    public async Task<Unit> Handle(DeleteMeasurementCommand request, CancellationToken cancellationToken)
    {
        var measurement = await _measurementRepository.GetByIdAsync(request.Id);
        if (measurement == null)
        {
            throw new EntityNotFoundException(nameof(Measurement), request.Id);
        }

        var hasProducts = await _measurementRepository.HasProductsAsync(request.Id);
        if (hasProducts)
        {
            throw new InvalidOperationException("Cannot delete measurement because it is associated with existing products.");
        }

        await _measurementRepository.DeleteAsync(request.Id);

        return Unit.Value;
    }
}