// Firmeza.Application/Measurements/Queries/GetMeasurements/GetMeasurementsQueryHandler.cs

using Application.Measurements.DTOs;
using Application.Products.Queries.GetMeasurements;
using AutoMapper;
using Domain.Interfaces;
using MediatR;

namespace Application.Measurements.Queries.GetMeasurements;

public class GetMeasurementsQueryHandler : IRequestHandler<GetMeasurementsQuery, IEnumerable<MeasurementDto>>
{
    private readonly IMeasurementRepository _measurementRepository;
    private readonly IMapper _mapper;

    public GetMeasurementsQueryHandler(
        IMeasurementRepository measurementRepository,
        IMapper mapper)
    {
        _measurementRepository = measurementRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<MeasurementDto>> Handle(GetMeasurementsQuery request, CancellationToken cancellationToken)
    {
        var measurements = request.OnlyActive
            ? await _measurementRepository.GetActiveAsync()
            : await _measurementRepository.GetAllAsync();

        return _mapper.Map<IEnumerable<MeasurementDto>>(measurements);
    }
}