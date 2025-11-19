// Firmeza.Application/Suppliers/MappingProfiles/SupplierMappingProfile.cs

using AutoMapper;
using Domain.Entities;
using Firmeza.Application.Suppliers.DTOs;

namespace Application.Suppliers.MappingProfiles;

public class SupplierMappingProfile : Profile
{
    public SupplierMappingProfile()
    {
        CreateMap<Supplier, SupplierDto>();
    }
}