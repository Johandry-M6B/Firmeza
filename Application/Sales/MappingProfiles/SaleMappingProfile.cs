using Application.Sales.DTOs;
using AutoMapper;
using Domain.Entities;

namespace Application.Sales.MappingProfiles;

public class SaleMappingProfile : Profile
{
    public SaleMappingProfile()
    {
        CreateMap<Sale, SaleDto>()
            .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.Customer.FullName));

        CreateMap<SalesDetail, SaleDetailDto>()
            .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name));

        CreateMap<PaymentSale, PaymentSaleDto>();
    }
}