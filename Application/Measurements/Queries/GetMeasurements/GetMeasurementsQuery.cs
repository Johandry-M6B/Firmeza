using System.Diagnostics.Metrics;
using Firmeza.Application.Measurements.DTOs;
using MediatR;

namespace Application.Products.Queries.GetMeasurements;

public class GetMeasurementsQuery : IRequest<IEnumerable<MeasurementDto>>
{
    public bool OnlyActive { get; set; } = true;    
}