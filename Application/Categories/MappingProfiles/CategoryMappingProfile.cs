// Firmeza.Application/Categories/MappingProfiles/CategoryMappingProfile.cs

using Application.Categories.DTOs;
using AutoMapper;
using Domain.Entities;

namespace Application.Categories.MappingProfiles;

public class CategoryMappingProfile : Profile
{
    public CategoryMappingProfile()
    {
        CreateMap<Category, CategoryDto>();
    }
}