using Application.Customers.DTOs;
using AutoMapper;
using Domain.Entities;

namespace Application.Customers.MappingProfiles;

public class CustomerMappingProfile : Profile
{
    public CustomerMappingProfile()
    {
        CreateMap<Customer, CustomerDto>();
    }
}