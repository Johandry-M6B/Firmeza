using Application.Measurements.DTOs;
using AutoMapper;
using Domain.Interfaces;
using MediatR;

namespace Application.Measurements.Queries.GetMeasurementById;

public class GetMeasurementByIdQueryHandler : IRequestHandler<GetMeasurementByIdQuery, MeasurementDto>
{
    private readonly IMeasurementRepository _measurementRepository;
    private readonly IMapper _mapper;

    public GetMeasurementByIdQueryHandler(
        IMeasurementRepository measurementRepository,
        IMapper mapper)
    {
        _measurementRepository = measurementRepository;
        _mapper = mapper;
    }

    public async Task<MeasurementDto?> Handle(GetMeasurementByIdQuery request, CancellationToken cancellationToken)
    {
        var measurement = await _measurementRepository.GetByIdAsync(request.Id);
        return measurement == null ? null : _mapper.Map<MeasurementDto>(measurement);
    }
}