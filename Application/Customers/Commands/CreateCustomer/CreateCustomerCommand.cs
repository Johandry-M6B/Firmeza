using Domain.Enums;
using MediatR;

namespace Application.Customers.Commands.CreateCustomer;

public class CreateCustomerCommand : IRequest<int>
{
    public TypeCustomer TypeCustomer { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string? Nit { get; set; }
    public string DocumentNumber { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public string? City { get; set; }
    public string? Country { get; set; }
    public decimal CreditLimit { get; set; }
    public int DaysToPay { get; set; }
    public decimal? SpecialDiscount { get; set; }
}