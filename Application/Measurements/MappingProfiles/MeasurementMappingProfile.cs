// Firmeza.Application/Measurements/MappingProfiles/MeasurementMappingProfile.cs

using Application.Measurements.DTOs;
using AutoMapper;
using Domain.Entities;

namespace Application.Measurements.MappingProfiles;

public class MeasurementMappingProfile : Profile
{
    public MeasurementMappingProfile()
    {
        CreateMap<Measurement, MeasurementDto>();
    }
}