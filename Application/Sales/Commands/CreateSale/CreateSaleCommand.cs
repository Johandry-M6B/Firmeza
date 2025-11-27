using Application.Sales.DTOs;
using Domain.Enums;
using MediatR;

namespace Application.Sales.Commands.CreateSale;

public class CreateSaleCommand : IRequest<int>
{
    public int CustomerId { get; set; }
    public PaymentFrom PaymentFrom { get; set; }
    public decimal Discount { get; set; }
    public string? DeliveryAddress { get; set; }
    public DateTime? DeliveryDate { get; set; }
    public string? Observations { get; set; }
    public List<CreateSaleDetailDto> Details { get; set; } = new();
}