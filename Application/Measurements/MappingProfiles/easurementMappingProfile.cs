// Firmeza.Application/Measurements/MappingProfiles/MeasurementMappingProfile.cs
using AutoMapper;
using Domain.Entities;
using Firmeza.Application.Measurements.DTOs;

namespace Firmeza.Application.Measurements.MappingProfiles;

public class MeasurementMappingProfile : Profile
{
    public MeasurementMappingProfile()
    {
        CreateMap<Measurement, MeasurementDto>();
    }
}