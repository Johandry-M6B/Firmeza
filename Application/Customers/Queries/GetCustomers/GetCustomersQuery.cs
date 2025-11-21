using Application.Customers.DTOs;
using Domain.Enums;
using MediatR;

namespace Application.Customers.Queries.GetCustomers;

public class GetCustomersQuery : IRequest<IEnumerable<CustomerDto>>
{
    public bool OnlyActive { get; set; } = true;
    public string? SearchTerm { get; set; }
    public TypeCustomer? Type { get; set; }
}