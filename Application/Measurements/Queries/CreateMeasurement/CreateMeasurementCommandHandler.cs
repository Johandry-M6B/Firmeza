using Domain.Interfaces;
using MediatR;
using Domain.Entities;

namespace Application.Measurements.Queries.CreateMeasurement;

public class CreateMeasurementCommandHandler : IRequestHandler<CreateMeasurementCommand,int >
{
    private readonly IMeasurementRepository _measurementRepository;

    public CreateMeasurementCommandHandler(IMeasurementRepository measurementRepository)
    {
        _measurementRepository = measurementRepository;
    }

    public async Task<int> Handle(CreateMeasurementCommand request, CancellationToken cancellationToken)
    {
        var measurement = new Measurement
        {
            Name = request.Name,
            Abbreviation = request.Abbreviation,
            Active = true
        };

        var created = await _measurementRepository.AddAsync(measurement);
        return created.Id;
    }
}