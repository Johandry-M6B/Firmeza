// Firmeza.Application/Suppliers/MappingProfiles/SupplierMappingProfile.cs

using Application.Suppliers.DTOs;
using AutoMapper;
using Domain.Entities;

namespace Application.Suppliers.MappingProfiles;

public class SupplierMappingProfile : Profile
{
    public SupplierMappingProfile()
    {
        CreateMap<Supplier, SupplierDto>();
    }
}