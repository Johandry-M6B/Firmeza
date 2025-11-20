using Application.Measurements.DTOs;
using MediatR;

namespace Application.Measurements.Queries.GetMeasurementById;

public class GetMeasurementByIdQuery : IRequest<MeasurementDto>
{
    public int Id { get; set; }
    
    public GetMeasurementByIdQuery(int id)
    {
        Id = id;
    }
}